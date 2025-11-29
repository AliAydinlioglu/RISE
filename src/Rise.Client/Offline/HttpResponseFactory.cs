using System.Net;
using System.Text;

namespace Rise.Client.Offline;

public static class HttpResponseFactory
{
    private const string DefaultMediaType = "application/json";

    public static HttpResponseMessage CreateResponse(HttpRequestMessage request, string? cachedContent)
    {
        if (cachedContent == null)
            return UnavailableResponse(request);
        
        return SuccessResponse(request, cachedContent);
    }

    private static HttpResponseMessage SuccessResponse(HttpRequestMessage request, string cachedContent)
    {
        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(cachedContent, Encoding.UTF8, DefaultMediaType),
            RequestMessage = request
        };

    }

    private static HttpResponseMessage UnavailableResponse(HttpRequestMessage request)
    {
        Log.Warning("Geen cached data beschikbaar voor '{RequestUri}'", request.RequestUri);
        return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
        {
            Content = new StringContent(
                "Offline en geen cached data beschikbaar",
                Encoding.UTF8,
                "text/plain"),
            RequestMessage = request
        };

    }
}