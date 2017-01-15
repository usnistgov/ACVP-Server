using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.Core
{
    public interface ITestCaseGeneratorFactoryFactory<in TTestVectorSet>
        where TTestVectorSet : ITestVectorSet
    {
        GenerateResponse BuildTestCases(TTestVectorSet testVectorSet);
    }
}
