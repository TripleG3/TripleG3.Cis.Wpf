namespace TripleG3.Cis.Wpf.Tests;

public sealed class TestStateService<T> : IStateService<T>
{
    private readonly TaskCompletionSource<StateValueFactory<T>> setAsyncCalled = new(TaskCreationOptions.RunContinuationsAsynchronously);
    private State<T> state;

    public TestStateService(State<T> state)
    {
        this.state = state;
    }

    public event EventHandler<State<T>>? StateChanged;

    public State<T> State => state;

    public int SetAsyncCallCount { get; private set; }

    public StateValueFactory<T>? CapturedValueFactory { get; private set; }

    public CancellationToken CapturedCancellationToken { get; private set; }

    public ValueTask<State<T>> SetAsync(StateValueFactory<T> valueFactory, CancellationToken cancellationToken)
    {
        SetAsyncCallCount++;
        CapturedValueFactory = valueFactory;
        CapturedCancellationToken = cancellationToken;
        setAsyncCalled.TrySetResult(valueFactory);
        return ValueTask.FromResult(state);
    }

    public void SetState(State<T> state)
    {
        this.state = state;
        StateChanged?.Invoke(this, state);
    }

    public Task<StateValueFactory<T>> WaitForSetAsyncCallAsync() => setAsyncCalled.Task.WaitAsync(TimeSpan.FromSeconds(1));
}