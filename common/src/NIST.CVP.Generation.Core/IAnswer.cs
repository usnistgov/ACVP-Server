using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.Core
{
    public interface IAnswer
    {
        int TestCaseId { get; set; }
        bool Pass { get; }
        bool Deferred { get; }
        string Reason { get; }
        TestCaseValidation Validate(ITestResult result);
    }
}
