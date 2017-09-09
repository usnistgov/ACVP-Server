using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.CMAC
{
    public class TestCaseGeneratorFactory<TTestGroup, TTestCase> : ITestCaseGeneratorFactory<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestCase>
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
                return new TestCaseGeneratorGen<TTestGroup, TTestCase>(_random800_90, cmac);
            }

            if (direction == "ver")
            {
                return new TestCaseGeneratorVer<TTestGroup, TTestCase>(_random800_90, cmac);
            }

            return new TestCaseGeneratorNull<TTestGroup, TTestCase>();
        }
    }
}
