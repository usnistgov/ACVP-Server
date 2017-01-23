using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA1
{
    public interface ITestCaseGeneratorFactory
    {
        ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator();
    }
}
