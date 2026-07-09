namespace TripleG3.Cis.Wpf;

public class StateBindingCommand<T>(StateBindingCommandParameter<T> stateBindingCommandParameter)
    : BindingCommand<T>(async _ => await stateBindingCommandParameter.StateService.SetAsync(
                            stateBindingCommandParameter.ValueFactory,
                            stateBindingCommandParameter.TokenFactory()),
                        _ => stateBindingCommandParameter.StateService.State.Status != StateStatus.Busy,
                        stateBindingCommandParameter.NotifyPropertyChanged)
{ }
