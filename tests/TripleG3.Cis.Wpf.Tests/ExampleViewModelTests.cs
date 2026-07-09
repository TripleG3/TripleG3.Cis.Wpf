namespace TripleG3.Cis.Wpf.Tests;

public sealed class ExampleViewModelTests
{
    [Fact]
    public void Constructor_WhenCreated_InitializesSetStringCommand()
    {
        //Arrange
        TestExampleStringService exampleStringService = new(new State<string>("initial", StateStatus.Ready, string.Empty));

        //Act
        ExampleViewModel actor = new(exampleStringService);

        //Assert
        Assert.NotNull(actor.SetStringCommand);
    }

    [Fact]
    public void SetStringCommandCanExecute_WhenServiceIsBusy_ReturnsFalse()
    {
        //Arrange
        TestExampleStringService exampleStringService = new(new State<string>("initial", StateStatus.Busy, string.Empty));
        ExampleViewModel actor = new(exampleStringService);

        //Act
        bool result = actor.SetStringCommand.CanExecute(null);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public async Task SetStringCommandExecute_WhenValueIsSet_PassesValueToServiceAsync()
    {
        //Arrange
        TestExampleStringService exampleStringService = new(new State<string>("initial", StateStatus.Ready, string.Empty));
        ExampleViewModel actor = new(exampleStringService)
        {
            Value = "updated"
        };

        //Act
        actor.SetStringCommand.Execute(null);
        await exampleStringService.WaitForSetAsyncCallAsync();

        //Assert
        Assert.Equal("updated", exampleStringService.CapturedSetStringValue);
    }
}