using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_ECB
{
    public interface IResultValidator
    {
        TestVectorValidation ValidateResults(List<ITestCaseValidator<TestCase>> testCaseValidators, List<TestCase> testResults);
    }
}
