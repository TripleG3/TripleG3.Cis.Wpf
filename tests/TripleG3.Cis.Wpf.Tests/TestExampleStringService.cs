namespace TripleG3.Cis.Wpf.Tests;

public sealed class TestExampleStringService : IExampleStringService
{
    private readonly TaskCompletionSource<object?> setAsyncCalled = new(TaskCreationOptions.RunContinuationsAsynchronously);
    private State<string> state;

    public TestExampleStringService(State<string> state)
    {
        this.state = state;
    }

    public event EventHandler<State<string>>? StateChanged;

    public State<string> State => state;

    public string? CapturedSetStringValue { get; private set; }

    public ValueTask<string> SetStringAsync(string value, CancellationToken cancellationToken)
    {
        CapturedSetStringValue = value;
        return ValueTask.FromResult($"Processed: {value}");
    }

    public async ValueTask<State<string>> SetAsync(StateValueFactory<string> valueFactory, CancellationToken cancellationToken)
    {
        string value = await valueFactory(cancellationToken);
        state = new State<string>(value, StateStatus.Ready, string.Empty);
        StateChanged?.Invoke(this, state);
        setAsyncCalled.TrySetResult(null);
        return state;
    }

    public Task WaitForSetAsyncCallAsync() => setAsyncCalled.Task.WaitAsync(TimeSpan.FromSeconds(1));
}