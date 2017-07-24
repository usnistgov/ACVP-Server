using System;
using NIST.CVP.Crypto.KeyWrap;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.KeyWrap
{
    public class TestCaseGeneratorEncryptTdes : ITestCaseGenerator<TestGroupTdes, TestCaseTdes>
    {
        private readonly IKeyWrapFactory _iKeyWrapFactory;
        private readonly IRandom800_90 _iRandom800_90;

        public TestCaseGeneratorEncryptTdes(IKeyWrapFactory iKeyWrapFactory, IRandom800_90 iRandom800_90)
        {
            _iKeyWrapFactory = iKeyWrapFactory;
            _iRandom800_90 = iRandom800_90;
        }

        public int NumberOfTestCasesToGenerate => 100;

        public TestCaseGenerateResponse Generate(TestGroupTdes @group, bool isSample)
        {
            var key = _iRandom800_90.GetRandomBitString(group.KeyLength);
            var plainText = _iRandom800_90.GetRandomBitString(64);
            var testCase = new TestCaseTdes
            {
                Key = key,
                PlainText = plainText
            };
            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse Generate(TestGroupTdes @group, TestCaseTdes testCase)
        {
            KeyWrapResult wrapResult = null;
            try
            {
                var keyWrap = _iKeyWrapFactory.GetKeyWrapInstance(group.KeyWrapType);

                wrapResult = keyWrap.Encrypt(testCase.Key, testCase.PlainText, @group.UseInverseCipher);
                if (!wrapResult.Success)
                {
                    ThisLogger.Warn(wrapResult.ErrorMessage);
                    {
                        return new TestCaseGenerateResponse(wrapResult.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                {
                    return new TestCaseGenerateResponse(ex.Message);
                }
            }
            testCase.CipherText = wrapResult.ResultingBitString;
            return new TestCaseGenerateResponse(testCase);
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}