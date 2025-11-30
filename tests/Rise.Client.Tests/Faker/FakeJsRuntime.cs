using Microsoft.JSInterop;

namespace Rise.Client.Faker;

public class FakeJsRuntime: IJSRuntime
{
    public int InvokeAsyncCallCount { get; private set; }
    public string? LastInvokedFunction { get; private set; }
    public object[]? LastInvokedArgs { get; private set; }

    public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args)
    {
        InvokeAsyncCallCount++;
        LastInvokedFunction = identifier;
        LastInvokedArgs = args;
        return ValueTask.FromResult(default(TValue)!);
    }

    public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
    {
        return InvokeAsync<TValue>(identifier, args);
    }

    public void Reset()
    {
        InvokeAsyncCallCount = 0;
        LastInvokedFunction = null;
        LastInvokedArgs = null;
    }
}