using NIST.CVP.Crypto.Common.Symmetric.KeyWrap;
using NIST.CVP.Crypto.KeyWrap;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KeyWrap
{
    public class TestCaseGeneratorFactory<TTestGroup, TTestCase> : ITestCaseGeneratorFactory<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase
        where TTestCase : TestCaseBase, new()

    {
        private readonly IKeyWrapFactory _iKeyWrapFactory;
        private readonly IRandom800_90 _iRandom800_90;

        public TestCaseGeneratorFactory(IKeyWrapFactory iKeyWrapFactory, IRandom800_90 iRandom800_90)
        {
            _iKeyWrapFactory = iKeyWrapFactory;
            _iRandom800_90 = iRandom800_90;
        }

        public ITestCaseGenerator<TTestGroup, TTestCase> GetCaseGenerator(TTestGroup testGroup)
        {
            switch (testGroup.Direction.ToLower())
            {
                case "encrypt":
                    return new TestCaseGeneratorEncrypt<TTestGroup, TTestCase>(_iKeyWrapFactory, _iRandom800_90);
                case "decrypt":
                    return new TestCaseGeneratorDecrypt<TTestGroup, TTestCase>(_iKeyWrapFactory, _iRandom800_90);
                default:
                    return new TestCaseGeneratorNull<TTestGroup, TTestCase>();
            }
        }
    }
}
