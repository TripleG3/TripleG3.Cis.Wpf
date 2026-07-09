namespace TripleG3.Cis.Wpf;

public interface IExampleStringService : IStateService<string>
{
    ValueTask<string> SetStringAsync(string value, CancellationToken cancellationToken);
}

public class ExampleStringService : StateService<string>, IExampleStringService
{
    private static int counter;

    public async ValueTask<string> SetStringAsync(string value, CancellationToken cancellationToken)
    {
        return (await SetAsync(async token =>
        {
            counter++;
            await Task.Delay(1000, token); // Simulate some asynchronous operation
            return $"Processed {counter}: {value}";
        }, cancellationToken)).Value;
    }
}

public class ExampleViewModel : StateServiceViewModel<string>
{    
    public ExampleViewModel(IExampleStringService exampleStateService) : base(exampleStateService)
    {
        StateBindingCommandParameter<string> stateBindingCommandParameter = new(
            exampleStateService,
            x => exampleStateService.SetStringAsync(Value, x),
            GetCancellationToken,
            this);

        SetStringCommand = new StateBindingCommand<string>(stateBindingCommandParameter);
    }

    public StateBindingCommand<string> SetStringCommand { get; }
}
