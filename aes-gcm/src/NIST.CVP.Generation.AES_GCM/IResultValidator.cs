using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_GCM
{
    public interface IResultValidator
    {
        TestVectorValidation ValidateResults(List<ITestCaseValidator<TestCase>> testCaseValidators, List<TestCase> testResults);
    }
}
