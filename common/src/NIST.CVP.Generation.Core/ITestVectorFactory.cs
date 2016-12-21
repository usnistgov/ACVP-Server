namespace NIST.CVP.Generation.Core
{
    public interface ITestVectorFactory<in TParameters>
        where TParameters : IParameters
    {
        ITestVectorSet BuildTestVectorSet(TParameters parameters);
    }
}
