namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Enums
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
