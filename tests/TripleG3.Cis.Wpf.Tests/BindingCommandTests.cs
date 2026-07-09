namespace TripleG3.Cis.Wpf.Tests;

public sealed class BindingCommandTests
{
    [Fact]
    public void Constructor_WhenExecuteIsNull_ThrowsArgumentNullException()
    {
        //Arrange
        TestNotifyPropertyChanged notifyPropertyChanged = new();

        //Act
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() =>
        {
            BindingCommand actor = new((Action<object?>)null!, _ => true, notifyPropertyChanged);
        });

        //Assert
        Assert.Equal("execute", exception.ParamName);
    }

    [Fact]
    public void Constructor_WhenCanExecuteIsNull_ThrowsArgumentNullException()
    {
        //Arrange
        TestNotifyPropertyChanged notifyPropertyChanged = new();

        //Act
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() =>
        {
            BindingCommand actor = new(_ => { }, (Func<object?, bool>)null!, notifyPropertyChanged);
        });

        //Assert
        Assert.Equal("canExecute", exception.ParamName);
    }

    [Fact]
    public void CanExecute_WhenParameterlessDelegateReturnsTrue_ReturnsTrue()
    {
        //Arrange
        TestNotifyPropertyChanged notifyPropertyChanged = new();
        BindingCommand actor = new(() => { }, () => true, notifyPropertyChanged);

        //Act
        bool result = actor.CanExecute(null);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void Execute_WhenParameterlessDelegateIsProvided_InvokesDelegate()
    {
        //Arrange
        bool executed = false;
        TestNotifyPropertyChanged notifyPropertyChanged = new();
        BindingCommand actor = new(() => executed = true, () => true, notifyPropertyChanged);

        //Act
        actor.Execute(null);

        //Assert
        Assert.True(executed);
    }

    [Fact]
    public void CanExecute_WhenParameterizedDelegateIsProvided_PassesParameter()
    {
        //Arrange
        object parameter = new();
        object? capturedParameter = null;
        TestNotifyPropertyChanged notifyPropertyChanged = new();
        BindingCommand actor = new(_ => { }, value =>
        {
            capturedParameter = value;
            return true;
        }, notifyPropertyChanged);

        //Act
        actor.CanExecute(parameter);

        //Assert
        Assert.Same(parameter, capturedParameter);
    }

    [Fact]
    public void Execute_WhenParameterizedDelegateIsProvided_PassesParameter()
    {
        //Arrange
        object parameter = new();
        object? capturedParameter = null;
        TestNotifyPropertyChanged notifyPropertyChanged = new();
        BindingCommand actor = new(value => capturedParameter = value, _ => true, notifyPropertyChanged);

        //Act
        actor.Execute(parameter);

        //Assert
        Assert.Same(parameter, capturedParameter);
    }

    [Fact]
    public void CanExecuteChanged_WhenNotifierRaisesPropertyChanged_RaisesEvent()
    {
        //Arrange
        bool eventRaised = false;
        TestNotifyPropertyChanged notifyPropertyChanged = new();
        BindingCommand actor = new(_ => { }, _ => true, notifyPropertyChanged);
        actor.CanExecuteChanged += (_, _) => eventRaised = true;

        //Act
        notifyPropertyChanged.RaisePropertyChanged();

        //Assert
        Assert.True(eventRaised);
    }
}