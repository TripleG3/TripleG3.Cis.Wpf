namespace TripleG3.Cis.Wpf.Tests;

public sealed class StateBindingCommandParameterTests
{
    [Fact]
    public void Empty_WhenRead_UsesEmptyStateService()
    {
        //Arrange
        StateBindingCommandParameter<string> actor = CreateActor();

        //Act
        StateBindingCommandParameter<string> result = actor.Empty;

        //Assert
        Assert.Same(IStateService<string>.Empty, result.StateService);
    }

    [Fact]
    public void Empty_WhenRead_UsesCancellationTokenNone()
    {
        //Arrange
        StateBindingCommandParameter<string> actor = CreateActor();

        //Act
        CancellationToken result = actor.Empty.TokenFactory();

        //Assert
        Assert.Equal(CancellationToken.None, result);
    }

    [Fact]
    public void Empty_WhenRead_UsesEmptyNotifier()
    {
        //Arrange
        StateBindingCommandParameter<string> actor = CreateActor();

        //Act
        INotifyPropertyChanged result = actor.Empty.NotifyPropertyChanged;

        //Assert
        Assert.Same(EmptyNotifyPropertyChanged.Instance, result);
    }

    [Fact]
    public async Task EmptyValueFactory_WhenInvoked_ReturnsDefaultAsync()
    {
        //Arrange
        StateBindingCommandParameter<string> actor = CreateActor();

        //Act
        string? result = await actor.Empty.ValueFactory(CancellationToken.None);

        //Assert
        Assert.Null(result);
    }

    private static StateBindingCommandParameter<string> CreateActor()
    {
        TestStateService<string> stateService = new(new State<string>("ready", StateStatus.Ready, string.Empty));
        TestNotifyPropertyChanged notifyPropertyChanged = new();
        return new StateBindingCommandParameter<string>(stateService, _ => ValueTask.FromResult("next"), () => CancellationToken.None, notifyPropertyChanged);
    }
}