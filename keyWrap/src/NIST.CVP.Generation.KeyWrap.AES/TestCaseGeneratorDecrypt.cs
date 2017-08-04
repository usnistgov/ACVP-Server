using System;
using NIST.CVP.Crypto.KeyWrap;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.KeyWrap.AES
{
    public class TestCaseGeneratorDecrypt : TestCaseGeneratorDecryptBase<TestGroup, TestCase>
    {

        public TestCaseGeneratorDecrypt(IKeyWrapFactory iKeyWrapFactory, IRandom800_90 iRandom800_90) : 
            base(iKeyWrapFactory, iRandom800_90)
        {
        }

        public override TestCaseGenerateResponse Generate(TestGroup @group, bool isSample)
        {
            var key = _iRandom800_90.GetRandomBitString(group.KeyLength);
            var plainText = _iRandom800_90.GetRandomBitString(group.PtLen);
            var testCase = new TestCase
            {
                Key = key,
                PlainText = plainText
            };
            return Generate(group, testCase);
        }

        public override TestCaseGenerateResponse Generate(TestGroup @group, TestCase testCase)
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

            SometimesMangleTestCaseForFailureTest(testCase);

            return new TestCaseGenerateResponse(testCase);
        }

        private void SometimesMangleTestCaseForFailureTest(TestCase testCase)
        {
            // Alter the tag 20% of the time for a "failure" test
            int option = _iRandom800_90.GetRandomInt(0, 5);
            if (option == 0)
            {
                testCase.CipherText = _iRandom800_90.GetDifferentBitStringOfSameSize(testCase.CipherText);
                testCase.FailureTest = true;
            }
        }


    }
}