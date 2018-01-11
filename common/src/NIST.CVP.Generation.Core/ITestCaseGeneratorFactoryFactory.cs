using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.Core
{
    public interface ITestCaseGeneratorFactoryFactory<in TTestVectorSet, TTestGroup, TTestCase>
        where TTestVectorSet : ITestVectorSet
        where TTestGroup : ITestGroup
        where TTestCase : ITestCase
    {
        GenerateResponse BuildTestCases(TTestVectorSet testVectorSet);
    }
}
