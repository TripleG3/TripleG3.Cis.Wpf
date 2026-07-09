namespace TripleG3.Cis.Wpf;

public abstract class StateServiceViewModel<T> : ViewModel
{
    protected readonly CancellationTokenSource cancellationTokenSource = new();
    protected readonly IStateService<T> stateService;
    private T value;
    public StateServiceViewModel(IStateService<T> stateService)
    {
        this.stateService = stateService;
        this.stateService.StateChanged += (sender, state) => Value = state.Value;
        value = stateService.State.Value;
    }
    public T Value
    {
        get => value;
        set 
        {
            if (value == null && this.value == null) return;
            if (value != null && value.Equals(this.value)) return;
            this.value = value;
            OnPropertyChanged();
        }
    }
    protected CancellationToken GetCancellationToken() => cancellationTokenSource.Token;
}

public interface IExampleStringService : IStateService<string>
{
    ValueTask<string> SetStringAsync(string value, CancellationToken cancellationToken);
}

public class ExampleViewModel : StateServiceViewModel<string>
{    
    public ExampleViewModel(IExampleStringService exampleStateService) : base(exampleStateService)
    {
        StateBindingCommandParameter<string> stateBindingCommandParameter = new(exampleStateService,
                                                                                async x => await exampleStateService.SetStringAsync(Value, x),
                                                                                GetCancellationToken,
                                                                                this);
        SetStringCommand = new StateBindingCommand<string>(stateBindingCommandParameter);
    }

    public StateBindingCommand<string> SetStringCommand { get; }
}

public class ExampleStringService : StateService<string>, IExampleStringService
{
    private static int counter = 0;
    public async ValueTask<string> SetStringAsync(string value, CancellationToken cancellationToken)
    {
        return (await SetAsync(async x =>
        {
            counter++;
            await Task.Delay(1000, x); // Simulate some asynchronous operation
            return $"Processed {counter}: {value}";
        }, cancellationToken)).Value;        
    }
}