using System.Collections.Generic;

namespace NIST.CVP.Generation.Core
{
    /// <summary>
    /// Provides <see cref="ITestGroupGenerator{TParameters}"/>s for a set of parameters.
    /// </summary>
    /// <typeparam name="TParameters">The parameters type.</typeparam>
    /// <typeparam name="TTestGroup">The test group type</typeparam>
    /// <typeparam name="TTestCase">The test case type</typeparam>
    public interface ITestGroupGeneratorFactory<in TParameters, out TTestGroup, TTestCase>
        where TParameters : IParameters
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>
    {
        /// <summary>
        /// Gets a collection of <see cref="ITestGroupGenerator{TParameters,TTestGroup,TTestCase}"/>.
        /// Separate group generators are often used for the different test types (AFT, KAT, VOT, etc)
        /// </summary>
        /// <returns></returns>
        IEnumerable<ITestGroupGenerator<TParameters, TTestGroup, TTestCase>> GetTestGroupGenerators(TParameters parameters);
    }
}