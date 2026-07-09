using System.ComponentModel;

namespace TripleG3.Cis.Wpf;

public sealed class EmptyNotifyPropertyChanged : INotifyPropertyChanged
{
    public static EmptyNotifyPropertyChanged Instance { get; }
    public event PropertyChangedEventHandler? PropertyChanged;
    private EmptyNotifyPropertyChanged() { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty)); }
    static EmptyNotifyPropertyChanged() { Instance = new EmptyNotifyPropertyChanged(); }
}