using NIST.CVP.Common.Oracle;
using NIST.CVP.Crypto.Common.Symmetric.KeyWrap;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KeyWrap
{
    public class TestCaseGeneratorFactory<TTestGroup, TTestCase> : ITestCaseGeneratorFactory<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase>
        where TTestCase : TestCaseBase<TTestGroup, TTestCase>, new()
    {
        private readonly IOracle _oracle;

        public TestCaseGeneratorFactory(IOracle oracle)
        {
            _oracle = oracle;
        }

        public ITestCaseGenerator<TTestGroup, TTestCase> GetCaseGenerator(TTestGroup testGroup)
        {
            switch (testGroup.Direction.ToLower())
            {
                case "encrypt":
                    return new TestCaseGeneratorEncrypt<TTestGroup, TTestCase>(_oracle);
                case "decrypt":
                    return new TestCaseGeneratorDecrypt<TTestGroup, TTestCase>(_oracle);
                default:
                    return new TestCaseGeneratorNull<TTestGroup, TTestCase>();
            }
        }
    }
}
