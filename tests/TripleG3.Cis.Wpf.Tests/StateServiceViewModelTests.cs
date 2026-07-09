namespace TripleG3.Cis.Wpf.Tests;

public sealed class StateServiceViewModelTests
{
    [Fact]
    public void Constructor_WhenServiceHasState_InitializesValue()
    {
        //Arrange
        TestStateService<string> stateService = new(new State<string>("initial", StateStatus.Ready, string.Empty));

        //Act
        TestStateServiceViewModel<string> actor = new(stateService);

        //Assert
        Assert.Equal("initial", actor.Value);
    }

    [Fact]
    public void StateChanged_WhenServiceRaisesState_UpdatesValue()
    {
        //Arrange
        TestStateService<string> stateService = new(new State<string>("initial", StateStatus.Ready, string.Empty));
        TestStateServiceViewModel<string> actor = new(stateService);

        //Act
        stateService.SetState(new State<string>("updated", StateStatus.Ready, string.Empty));

        //Assert
        Assert.Equal("updated", actor.Value);
    }

    [Fact]
    public void Value_WhenSetToSameValue_DoesNotRaisePropertyChanged()
    {
        //Arrange
        int propertyChangedCount = 0;
        TestStateService<string> stateService = new(new State<string>("initial", StateStatus.Ready, string.Empty));
        TestStateServiceViewModel<string> actor = new(stateService);
        actor.PropertyChanged += (_, _) => propertyChangedCount++;

        //Act
        actor.Value = "initial";

        //Assert
        Assert.Equal(0, propertyChangedCount);
    }

    [Fact]
    public void Value_WhenSetToDifferentValue_RaisesPropertyChanged()
    {
        //Arrange
        TestStateService<string> stateService = new(new State<string>("initial", StateStatus.Ready, string.Empty));
        TestStateServiceViewModel<string> actor = new(stateService);

        //Act
        void Act() => actor.Value = "updated";

        //Assert
        Assert.PropertyChanged(actor, nameof(StateServiceViewModel<string>.Value), Act);
    }
}