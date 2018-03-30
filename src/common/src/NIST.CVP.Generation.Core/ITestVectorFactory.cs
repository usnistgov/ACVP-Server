namespace NIST.CVP.Generation.Core
{
    /// <summary>
    /// Describes retrieving a <see cref="ITestVectorSet"/>
    /// </summary>
    /// <typeparam name="TParameters">The parameters to use in generating a test vector set</typeparam>
    public interface ITestVectorFactory<in TParameters>
        where TParameters : IParameters
    {
        /// <summary>
        /// Returns a <see cref="ITestVectorSet"/> based on <see cref="TParameters"/>
        /// </summary>
        /// <param name="parameters">The parameters to use in creating a <see cref="ITestVectorSet"/></param>
        /// <returns></returns>
        ITestVectorSet BuildTestVectorSet(TParameters parameters);
    }
}
