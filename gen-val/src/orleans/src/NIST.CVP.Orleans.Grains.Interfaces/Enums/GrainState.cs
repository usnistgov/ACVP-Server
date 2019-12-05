namespace NIST.CVP.Orleans.Grains.Interfaces.Enums
{
    public enum GrainState
    {
        Initialized,
        Working,
        CompletedWork,
        Faulted,
        ShouldDispose
    }
}