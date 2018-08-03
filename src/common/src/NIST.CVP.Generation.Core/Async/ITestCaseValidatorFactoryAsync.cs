using System.Collections.Generic;

namespace NIST.CVP.Generation.Core.Async
{
    /// <summary>
    /// Factory interface for getting <see cref="ITestCaseValidator{TTestGroup, TTestCase}"/>s 
    /// based on a <see cref="TTestVectorSet"/>
    /// </summary>
    /// <typeparam name="TTestVectorSet">The test vector set type.</typeparam>
    /// <typeparam name="TTestGroup">The test group type</typeparam>
    /// <typeparam name="TTestCase">The test case type.</typeparam>
    public interface ITestCaseValidatorFactoryAsync<in TTestVectorSet, TTestGroup, in TTestCase>
        where TTestVectorSet : ITestVectorSet<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>
    {
        /// <summary>
        /// Returns a collection of <see cref="ITestCaseValidator{TTestGroup, TTestCase}"/>s
        /// based on the provided <see cref="TTestVectorSet"/>.
        /// </summary>
        /// <param name="testVectorSet">The test vector set to retrieve validators for each test case</param>
        /// <returns></returns>
        IEnumerable<ITestCaseValidatorAsync<TTestGroup, TTestCase>> GetValidators(TTestVectorSet testVectorSet);
    }
}