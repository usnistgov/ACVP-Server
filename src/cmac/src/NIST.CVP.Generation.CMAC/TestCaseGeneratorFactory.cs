using System;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.CMAC
{
    public class TestCaseGeneratorFactory<TTestCaseGeneratorGen, TTestCaseGeneratorVer, TTestGroup, TTestCase> 
        : ITestCaseGeneratorFactoryAsync<TTestGroup, TTestCase>
        where TTestCaseGeneratorGen : TestCaseGeneratorGenBase<TTestGroup, TTestCase>
        where TTestCaseGeneratorVer : TestCaseGeneratorVerBase<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase>
        where TTestCase : TestCaseBase<TTestGroup, TTestCase>, new()
    {
        private readonly IOracle _oracle;

        public TestCaseGeneratorFactory(IOracle oracle)
        {
            _oracle = oracle;
        }

        public ITestCaseGeneratorAsync<TTestGroup, TTestCase> GetCaseGenerator(TTestGroup testGroup)
        {
            var direction = testGroup.Function.ToLower();

            if (direction == "gen")
            {
                return (TTestCaseGeneratorGen)Activator.CreateInstance(typeof(TTestCaseGeneratorGen), _oracle);
            }

            if (direction == "ver")
            {
                return (TTestCaseGeneratorVer)Activator.CreateInstance(typeof(TTestCaseGeneratorVer), _oracle);
            }

            return new TestCaseGeneratorNull<TTestGroup, TTestCase>();
        }
    }
}
