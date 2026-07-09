namespace TripleG3.Cis.Wpf.Tests;

public sealed class BindingCommandGenericTests
{
    [Fact]
    public void CanExecute_WhenTypedDelegateAllowsParameter_ReturnsTrue()
    {
        //Arrange
        TestNotifyPropertyChanged notifyPropertyChanged = new();
        BindingCommand<string> actor = new(_ => { }, value => value == "allowed", notifyPropertyChanged);

        //Act
        bool result = actor.CanExecute("allowed");

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void Execute_WhenTypedParameterIsProvided_PassesTypedParameter()
    {
        //Arrange
        string? capturedValue = null;
        TestNotifyPropertyChanged notifyPropertyChanged = new();
        BindingCommand<string> actor = new(value => capturedValue = value, _ => true, notifyPropertyChanged);

        //Act
        actor.Execute("typed value");

        //Assert
        Assert.Equal("typed value", capturedValue);
    }
}