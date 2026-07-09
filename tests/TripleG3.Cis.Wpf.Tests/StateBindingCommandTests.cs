namespace TripleG3.Cis.Wpf.Tests;

public sealed class StateBindingCommandTests
{
    [Fact]
    public void CanExecute_WhenStateIsReady_ReturnsTrue()
    {
        //Arrange
        TestNotifyPropertyChanged notifyPropertyChanged = new();
        TestStateService<string> stateService = new(new State<string>("ready", StateStatus.Ready, string.Empty));
        StateBindingCommandParameter<string> commandParameter = new(stateService, _ => ValueTask.FromResult("next"), () => CancellationToken.None, notifyPropertyChanged);
        StateBindingCommand<string> actor = new(commandParameter);

        //Act
        bool result = actor.CanExecute(null);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void CanExecute_WhenStateIsBusy_ReturnsFalse()
    {
        //Arrange
        TestNotifyPropertyChanged notifyPropertyChanged = new();
        TestStateService<string> stateService = new(new State<string>("busy", StateStatus.Busy, string.Empty));
        StateBindingCommandParameter<string> commandParameter = new(stateService, _ => ValueTask.FromResult("next"), () => CancellationToken.None, notifyPropertyChanged);
        StateBindingCommand<string> actor = new(commandParameter);

        //Act
        bool result = actor.CanExecute(null);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Execute_WhenCalled_InvokesStateServiceSetAsync()
    {
        //Arrange
        TestNotifyPropertyChanged notifyPropertyChanged = new();
        TestStateService<string> stateService = new(new State<string>("ready", StateStatus.Ready, string.Empty));
        StateBindingCommandParameter<string> commandParameter = new(stateService, _ => ValueTask.FromResult("next"), () => CancellationToken.None, notifyPropertyChanged);
        StateBindingCommand<string> actor = new(commandParameter);

        //Act
        actor.Execute(null);
        await stateService.WaitForSetAsyncCallAsync();

        //Assert
        Assert.Equal(1, stateService.SetAsyncCallCount);
    }

    [Fact]
    public async Task Execute_WhenCalled_PassesValueFactoryToStateServiceAsync()
    {
        //Arrange
        TestNotifyPropertyChanged notifyPropertyChanged = new();
        TestStateService<string> stateService = new(new State<string>("ready", StateStatus.Ready, string.Empty));
        StateValueFactory<string> valueFactory = _ => ValueTask.FromResult("next");
        StateBindingCommandParameter<string> commandParameter = new(stateService, valueFactory, () => CancellationToken.None, notifyPropertyChanged);
        StateBindingCommand<string> actor = new(commandParameter);

        //Act
        actor.Execute(null);
        StateValueFactory<string> result = await stateService.WaitForSetAsyncCallAsync();

        //Assert
        Assert.Same(valueFactory, result);
    }

    [Fact]
    public async Task Execute_WhenCalled_PassesTokenFactoryTokenToStateServiceAsync()
    {
        //Arrange
        using CancellationTokenSource cancellationTokenSource = new();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        TestNotifyPropertyChanged notifyPropertyChanged = new();
        TestStateService<string> stateService = new(new State<string>("ready", StateStatus.Ready, string.Empty));
        StateBindingCommandParameter<string> commandParameter = new(stateService, _ => ValueTask.FromResult("next"), () => cancellationToken, notifyPropertyChanged);
        StateBindingCommand<string> actor = new(commandParameter);

        //Act
        actor.Execute(null);
        await stateService.WaitForSetAsyncCallAsync();

        //Assert
        Assert.Equal(cancellationToken, stateService.CapturedCancellationToken);
    }
}