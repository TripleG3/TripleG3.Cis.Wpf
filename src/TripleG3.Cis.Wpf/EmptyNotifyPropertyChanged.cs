using System.ComponentModel;

namespace TripleG3.Cis.Wpf;

public sealed class EmptyNotifyPropertyChanged : INotifyPropertyChanged
{
    public static EmptyNotifyPropertyChanged Instance { get; } = new();

    public event PropertyChangedEventHandler? PropertyChanged
    {
        add { }
        remove { }
    }

    private EmptyNotifyPropertyChanged() { }
}