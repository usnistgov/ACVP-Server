using System.Collections.Generic;

namespace NIST.CVP.Generation.Core
{
    /// <summary>
    /// Factory interface for getting <see cref="ITestCaseValidator{TTestCase}"/>s 
    /// based on a <see cref="TTestVectorSet"/>
    /// </summary>
    /// <typeparam name="TTestVectorSet">The test vector set type.</typeparam>
    /// <typeparam name="TTestCase">The test case type.</typeparam>
    public interface ITestCaseValidatorFactory<in TTestVectorSet, in TTestCase>
        where TTestVectorSet : ITestVectorSet
        where TTestCase : ITestCase
    {
        /// <summary>
        /// Returns a collection of <see cref="ITestCaseValidator{TTestCase}"/>s
        /// based on the provided <see cref="TTestVectorSet"/>.
        /// </summary>
        /// <param name="testVectorSet">The test vector set to retrieve validators for each test case</param>
        /// <param name="suppliedResults">
        /// TODO this may be obsolete since implementing <see cref="IDeferredTestCaseResolver{TTestGroup,TTestCase,TCryptoResult}"/>, confirm?
        /// </param>
        /// <returns></returns>
        IEnumerable<ITestCaseValidator<TTestCase>> GetValidators(TTestVectorSet testVectorSet, IEnumerable<TTestCase> suppliedResults);
    }
}
