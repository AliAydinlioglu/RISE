using Rise.Client.Offline;

namespace Rise.Client.Faker;

public class FakeConnectionService: IConnectionService
{
    private readonly List<Action<bool>> _subscribers = new();
    
    public bool IsOnline { get; private set; } = true;
    public int InitializeAsyncCallCount { get; private set; }
    public int SubscriberCount => _subscribers.Count;

    public event Action<bool>? ConnectionStateChanged
    {
        add
        {
            if (value != null)
                _subscribers.Add(value);
        }
        remove
        {
            if (value != null)
                _subscribers.Remove(value);
        }
    }

    public Task InitializeAsync()
    {
        InitializeAsyncCallCount++;
        return Task.CompletedTask;
    }

    public void SetOnline(bool isOnline)
    {
        IsOnline = isOnline;
    }

    public void SimulateConnectionChange(bool isOnline)
    {
        IsOnline = isOnline;
        foreach (var subscriber in _subscribers)
        {
            subscriber?.Invoke(isOnline);
        }
    }

    public void Reset()
    {
        IsOnline = true;
        InitializeAsyncCallCount = 0;
        _subscribers.Clear();
    }

}