using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public interface IDeferredTestCaseGenerator<in TTestGroup, in TTestCase> : ITestCaseGenerator<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup
        where TTestCase : ITestCase
    {
        TestCaseGenerateResponse CompleteDeferredTestCase(TTestGroup group, TTestCase testCase);
        TestCaseGenerateResponse RecombineTestCases(TTestGroup group, TTestCase resultTestCase, TTestCase promptTestCase);
    }
}
