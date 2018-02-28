using System;
using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Crypto.Common.MAC.CMAC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.CMAC
{
    public class TestCaseGeneratorFactory<TTestCaseGeneratorGen, TTestGroup, TTestCase> : ITestCaseGeneratorFactory<TTestGroup, TTestCase>
        where TTestCaseGeneratorGen : TestCaseGeneratorGenBase<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase
        where TTestCase : TestCaseBase, new()
    {
        private readonly IRandom800_90 _random800_90;
        private readonly ICmacFactory _algoFactory;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, ICmacFactory algoFactory)
        {
            _algoFactory = algoFactory;
            _random800_90 = random800_90;
        }

        public ITestCaseGenerator<TTestGroup, TTestCase> GetCaseGenerator(TTestGroup testGroup)
        {
            var direction = testGroup.Function.ToLower();
            ICmac cmac = _algoFactory.GetCmacInstance(testGroup.CmacType);

            if (direction == "gen")
            {
                return (TTestCaseGeneratorGen)Activator.CreateInstance(typeof(TTestCaseGeneratorGen), _random800_90, cmac);
            }

            if (direction == "ver")
            {
                return new TestCaseGeneratorVer<TTestGroup, TTestCase>(_random800_90, cmac);
            }

            return new TestCaseGeneratorNull<TTestGroup, TTestCase>();
        }

        public ICmac GetCmac(TTestGroup testGroup)
        {
            return _algoFactory.GetCmacInstance(testGroup.CmacType);
        }
    }
}
