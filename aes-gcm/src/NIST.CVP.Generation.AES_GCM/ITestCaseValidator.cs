using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_GCM
{
    public interface ITestCaseValidator
    {
        TestCaseValidation Validate(TestCase suppliedResult);
        int TestCaseId { get; }
    }
}
