namespace TripleG3.Cis.Wpf.Tests;

public sealed class TestStateServiceViewModel<T> : StateServiceViewModel<T>
{
    public TestStateServiceViewModel(IStateService<T> stateService) : base(stateService)
    {
    }
}