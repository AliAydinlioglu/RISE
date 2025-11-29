namespace Rise.Client.Faker;

public class FakeHttpMessageHandler : HttpMessageHandler
{
    private HttpResponseMessage? _responseToReturn;
    private Exception? _exceptionToThrow;

    public void SetResponse(HttpResponseMessage response)
    {
        _responseToReturn = response;
        _exceptionToThrow = null;
    }

    public void ThrowException(Exception exception)
    {
        _exceptionToThrow = exception;
        _responseToReturn = null;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, 
        CancellationToken cancellationToken)
    {
        if (_exceptionToThrow != null)
        {
            throw _exceptionToThrow;
        }

        if (_responseToReturn != null)
        {
            return Task.FromResult(_responseToReturn);
        }

        return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK));
    }
}