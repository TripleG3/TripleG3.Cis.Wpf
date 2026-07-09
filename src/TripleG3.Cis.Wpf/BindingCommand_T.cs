using System.ComponentModel;

namespace TripleG3.Cis.Wpf;

/// <summary>
/// Strongly-typed variant of <see cref="BindingCommand"/> that uses a parameter of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the command parameter.</typeparam>
public class BindingCommand<T>(Action<T?> execute, Func<T?, bool> canExecute, INotifyPropertyChanged notifyPropertyChanged)
    : BindingCommand(parameter => execute((T?)parameter), parameter => canExecute((T?)parameter), notifyPropertyChanged)
{ }
