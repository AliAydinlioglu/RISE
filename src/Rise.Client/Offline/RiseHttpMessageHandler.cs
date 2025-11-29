namespace Rise.Client.Offline;

public class RiseHttpMessageHandler(ICacheService cacheService) : DelegatingHandler
{

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        // Our application only handles GET requests at this point
        if (!ShouldCache(request))
            return await base.SendAsync(request, cancellationToken);

        var cacheKey = GenerateCacheKey(request);
        var response = await SendRequestAsync(request, cancellationToken, cacheKey);

        return response ?? await GetCachedResponseAsync(request, cacheKey);
    }

    private async Task<HttpResponseMessage?> SendRequestAsync(HttpRequestMessage request,
        CancellationToken cancellationToken, string cacheKey)
    {
        try
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode) return null;

            await CacheResponseAsync(cacheKey, response, cancellationToken);
            return response;
        }
        catch (Exception e)
        {
            Log.Error(e, "An error occured while trying to send the request");
            return null;
        }
    }

    private async Task CacheResponseAsync(string cacheKey, HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        await cacheService.SetAsync(cacheKey, content);
    }

    private async Task<HttpResponseMessage> GetCachedResponseAsync(HttpRequestMessage request, string cacheKey)
    {
        var cachedContent = await cacheService.GetAsync(cacheKey);
        return HttpResponseFactory.CreateResponse(request, cachedContent);
    }

    private static bool ShouldCache(HttpRequestMessage request)
    {
        return request.Method == HttpMethod.Get;
    }

    private static string GenerateCacheKey(HttpRequestMessage request)
    {
        return request.RequestUri?.ToString() ?? string.Empty;
    }

}