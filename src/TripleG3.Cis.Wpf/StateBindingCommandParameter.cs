using System.ComponentModel;

namespace TripleG3.Cis.Wpf;

public record StateBindingCommandParameter<T>(IStateService<T> StateService, StateValueFactory<T> ValueFactory, Func<CancellationToken> TokenFactory, INotifyPropertyChanged NotifyPropertyChanged)
{
    public StateBindingCommandParameter<T> Empty { get; } = new StateBindingCommandParameter<T>(IStateService<T>.Empty, static x => ValueTask.FromResult<T>(default!), () => CancellationToken.None, EmptyNotifyPropertyChanged.Instance);
}
