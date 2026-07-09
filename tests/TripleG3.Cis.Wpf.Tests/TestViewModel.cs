namespace TripleG3.Cis.Wpf.Tests;

public sealed class TestViewModel : ViewModel
{
    private string value = string.Empty;

    public string Value
    {
        get => value;
        set
        {
            if (value == this.value) return;
            this.value = value;
            OnPropertyChanged();
        }
    }
}