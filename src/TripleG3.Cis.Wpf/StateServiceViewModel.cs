namespace TripleG3.Cis.Wpf;

public abstract class StateServiceViewModel<T> : ViewModel
{
    protected readonly CancellationTokenSource cancellationTokenSource = new();
    protected readonly IStateService<T> stateService;
    private T value;
    public StateServiceViewModel(IStateService<T> stateService)
    {
        this.stateService = stateService;
        this.stateService.StateChanged += (_, state) => Value = state.Value;
        value = stateService.State.Value;
    }
    public T Value
    {
        get => value;
        set
        {
            if (EqualityComparer<T>.Default.Equals(value, this.value)) return;
            this.value = value;
            OnPropertyChanged();
        }
    }
    protected CancellationToken GetCancellationToken() => cancellationTokenSource.Token;
}
