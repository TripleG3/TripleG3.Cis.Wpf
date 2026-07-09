namespace TripleG3.Cis.Wpf.Tests;

public sealed class ViewModelTests
{
    [Fact]
    public void PropertySetter_WhenValueChanges_RaisesPropertyChanged()
    {
        //Arrange
        TestViewModel actor = new();

        //Act
        void Act() => actor.Value = "updated";

        //Assert
        Assert.PropertyChanged(actor, nameof(TestViewModel.Value), Act);
    }
}