using System.Collections.Generic;

namespace NIST.CVP.Generation.Core
{
    /// <summary>
    /// Provides a means of validating IUT algorithm responses against the server's expected values.
    /// </summary>
    /// <typeparam name="TTestCase">Specific test case type in which to perform validation</typeparam>
    public interface IResultValidator<TTestCase>
        where TTestCase : ITestCase
    {
        /// <summary>
        /// Perform validation utilizing provided <see cref="ITestCaseValidator{TTestCase}"/> against IUT provided <see cref="TTestCase"/>s
        /// </summary>
        /// <param name="testCaseValidators">The validators to use for each test case</param>
        /// <param name="testResults">The IUT supplied results to validate.</param>
        /// <returns></returns>
        TestVectorValidation ValidateResults(IEnumerable<ITestCaseValidator<TTestCase>> testCaseValidators, IEnumerable<TTestCase> testResults);
    }
}
