namespace TripleG3.Cis.Wpf.Tests;

public sealed class EmptyNotifyPropertyChangedTests
{
    [Fact]
    public void Instance_WhenRequestedMultipleTimes_ReturnsSameInstance()
    {
        //Arrange
        EmptyNotifyPropertyChanged actor = EmptyNotifyPropertyChanged.Instance;

        //Act
        EmptyNotifyPropertyChanged result = EmptyNotifyPropertyChanged.Instance;

        //Assert
        Assert.Same(actor, result);
    }

    [Fact]
    public void PropertyChanged_WhenHandlerIsAddedAndRemoved_DoesNotThrow()
    {
        //Arrange
        EmptyNotifyPropertyChanged actor = EmptyNotifyPropertyChanged.Instance;
        void Handler(object? sender, PropertyChangedEventArgs args) { }

        //Act
        Exception? exception = Record.Exception(() =>
        {
            actor.PropertyChanged += Handler;
            actor.PropertyChanged -= Handler;
        });

        //Assert
        Assert.Null(exception);
    }
}