using Rise.Services.Notifications;
using Rise.Shared.Notifications;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;

namespace Rise.Server.Endpoints.Notifications;

public class Stream: EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/api/notifications/stream");
        //authentication will be validated by its given access token in url
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        try
        {
            await Send.EventStreamAsync("Notification", GetDataStream(ct), ct);
        }
        catch (OperationCanceledException)
        {
            Log.Information("Notification event stream connection cancelled by client");
        }
        catch (Exception ex)
        {
            Log.Error($"Notification event stream: {ex}");
        }
    }

    private string GetUser()
    {
        //SSE endpoints needs access_token thru url, is not set in header
        var token = HttpContext.Request.Query["access_token"].ToString();
        //this is not good, its a workaround
        if (string.IsNullOrEmpty(token))
            throw new UnauthorizedAccessException();

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        // user id claim
        var userName = jwt.Claims.FirstOrDefault(c => c.Type.ToLower().Contains("upn"))?.Value;
        if (string.IsNullOrEmpty(userName))
            throw new UnauthorizedAccessException();

        return userName;
    }

    private async IAsyncEnumerable<NotificationDto.Index> GetDataStream(
        [EnumeratorCancellation] CancellationToken ct)
    {
        var reader = EventStreamNotification.Subscribe(GetUser()).Reader;

        while (!ct.IsCancellationRequested)
        {
            NotificationDto.Index msg;

            try
            {
                msg = await reader.ReadAsync(ct);
            }
            catch (OperationCanceledException)
            {
                yield break;
            }

            yield return msg;
        }
    }



}
