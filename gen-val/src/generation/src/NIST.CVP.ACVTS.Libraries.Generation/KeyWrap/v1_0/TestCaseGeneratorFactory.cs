using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;

namespace NIST.CVP.ACVTS.Libraries.Generation.KeyWrap.v1_0
{
    public class TestCaseGeneratorFactory<TTestGroup, TTestCase> : ITestCaseGeneratorFactoryAsync<TTestGroup, TTestCase>
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
