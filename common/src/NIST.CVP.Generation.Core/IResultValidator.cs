using System.Collections.Generic;

namespace NIST.CVP.Generation.Core
{
    public interface IResultValidator<TTestCase>
        where TTestCase : ITestCase
    {
        TestVectorValidation ValidateResults(IEnumerable<ITestCaseValidator<TTestCase>> testCaseValidators, IEnumerable<TTestCase> testResults);
    }
}
