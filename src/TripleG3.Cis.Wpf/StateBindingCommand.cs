namespace TripleG3.Cis.Wpf;

public class StateBindingCommand<T>(StateBindingCommandParameter<T> stateBindingCommandParameter)
    : BindingCommand<T>(async x => await stateBindingCommandParameter.StateService.SetAsync(async x => await stateBindingCommandParameter.ValueFactory(x), stateBindingCommandParameter.TokenFactory()),
                        x => stateBindingCommandParameter.StateService.State.Status != StateStatus.Busy,
                        stateBindingCommandParameter.NotifyPropertyChanged)
{ }
