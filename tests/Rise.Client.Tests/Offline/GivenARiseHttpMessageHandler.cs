using Rise.Client.Faker;
using Shouldly;

namespace Rise.Client.Offline;

public class GivenARiseHttpMessageHandler :IDisposable
{
    private readonly FakeCacheService _fakeCacheService;
    private readonly FakeHttpMessageHandler _fakeInnerHandler;
    private readonly HttpClient _httpClient;

    public GivenARiseHttpMessageHandler()
    {
        _fakeCacheService = new FakeCacheService();
        _fakeInnerHandler = new FakeHttpMessageHandler();
        var handler = new RiseHttpMessageHandler(_fakeCacheService)
        {
            InnerHandler = _fakeInnerHandler
        };
        _httpClient = new HttpClient(handler);
    }
    
    [Fact(DisplayName = "When GET received, then response should be cached")]
    public async Task SendAsync_WithSuccessfulGetRequest_ShouldCacheResponse()
    {
        var url = "https://api.example.com/data";
        var responseContent = "{\"id\":1,\"name\":\"test\"}";
        var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
        {
            Content = new StringContent(responseContent)
        };

        _fakeInnerHandler.SetResponse(response);

        var result = await _httpClient.GetAsync(url);

        result.IsSuccessStatusCode.ShouldBeTrue();
        _fakeCacheService.SetAsyncCallCount.ShouldBe(1);
        _fakeCacheService.LastSetKey.ShouldBe(url);
        _fakeCacheService.LastSetValue.ShouldBe(responseContent);
    }
    
    [Fact(DisplayName = "When http-method other than get received, then call should be delegated to base handler")]
    public async Task SendAsync_WithNonGetRequest_ShouldBypassCache()
    {
        var url = "https://api.example.com/data";
        var expectedResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        
        _fakeInnerHandler.SetResponse(expectedResponse);

        var result = await _httpClient.PostAsync(url, new StringContent(""));

        result.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        _fakeCacheService.GetAsyncCallCount.ShouldBe(0);
        _fakeCacheService.SetAsyncCallCount.ShouldBe(0);
    }


    [Fact(DisplayName = "When http call fails but call is cached, then the cached response should be returned")]
    public async Task SendAsync_WhenRequestFailsAndCacheExists_ShouldReturnCachedContent()
    {
        var url = "https://api.example.com/data";
        var cachedContent = "{\"id\":1,\"name\":\"cached\"}";

        _fakeInnerHandler.ThrowException(new HttpRequestException("Network error"));
        _fakeCacheService.SetCachedValue(url, cachedContent);

        var result = await _httpClient.GetAsync(url);

        result.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        var content = await result.Content.ReadAsStringAsync();
        content.ShouldBe(cachedContent);
        result.Content.Headers.ContentType?.MediaType.ShouldBe("application/json");
    }

    [Fact(DisplayName = "When http call fails but call is cached, then service unavailable should be returned")]
    public async Task SendAsync_WhenRequestFailsAndNoCacheExists_ShouldReturnServiceUnavailable()
    {
        var url = "https://api.example.com/data";

        _fakeInnerHandler.ThrowException(new HttpRequestException("Network error"));

        var result = await _httpClient.GetAsync(url);

        result.StatusCode.ShouldBe(System.Net.HttpStatusCode.ServiceUnavailable);
        var content = await result.Content.ReadAsStringAsync();
        content.ShouldBe("Offline en geen cached data beschikbaar");
    }
    
    public void Dispose()
    {
        _fakeCacheService.Clear();
    }
}