using System.Collections.Generic;

namespace NIST.CVP.Generation.Core
{
    public interface IResultValidator<TTestCase>
        where TTestCase : ITestCase
    {
        TestVectorValidation ValidateResults(List<ITestCaseValidator<TTestCase>> testCaseValidators, List<TTestCase> testResults);
    }
}
