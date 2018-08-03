using System.Threading.Tasks;

namespace NIST.CVP.Generation.Core.Async
{
    /// <summary>
    /// Generates test cases.  Performs crypto operations unless deferred or a known answer test.
    /// </summary>
    /// <typeparam name="TTestGroup">The test group information.</typeparam>
    /// <typeparam name="TTestCase">The test case type.</typeparam>
    public interface ITestCaseGeneratorAsync<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>
    {
        /// <summary>
        /// The number of test cases to generate for this test type and group.
        /// </summary>
        int NumberOfTestCasesToGenerate { get; }
        /// <summary>
        /// Generates a <see cref="TTestCase"/>.
        /// </summary>
        /// <param name="group">The test group information</param>
        /// <param name="isSample">Is this a sample test?</param>
        /// <returns></returns>
        Task<TestCaseGenerateResponse<TTestGroup, TTestCase>> GenerateAsync(TTestGroup @group, bool isSample);
    }
}