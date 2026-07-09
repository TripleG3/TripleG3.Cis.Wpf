namespace TripleG3.Cis.Wpf.Tests;

public sealed class TestNotifyPropertyChanged : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public void RaisePropertyChanged() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RaisePropertyChanged)));
}