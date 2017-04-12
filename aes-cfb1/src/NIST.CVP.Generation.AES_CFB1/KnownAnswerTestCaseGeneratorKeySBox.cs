using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.AES_CFB1;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_CFB1
{
    public class KnownAnswerTestCaseGeneratorKeySBox : IKnownAnswerTestCaseGenerator<TestGroup, TestCase>
    {
        public MultipleTestCaseGenerateResponse<TestCase> Generate(TestGroup testGroup)
        {
            List<BitOrientedAlgoArrayResponse> data = new List<BitOrientedAlgoArrayResponse>();

            switch (testGroup.KeyLength)
            {
                case 128:
                    data = KATDataCFB1.GetKeySBox128BitKey();
                    break;
                case 192:
                    data = KATDataCFB1.GetKeySBox192BitKey();
                    break;
                case 256:
                    data = KATDataCFB1.GetKeySBox256BitKey();
                    break;
                default:
                    return new MultipleTestCaseGenerateResponse<TestCase>($"Invalid {nameof(testGroup.KeyLength)} of {testGroup.KeyLength}");
            }
            
            var testCases = data.Select(s => new TestCase()
            {
                IV = s.IV,
                Key = s.Key,
                PlainText = s.PlainText,
                CipherText = s.CipherText
            }).ToList();

            return new MultipleTestCaseGenerateResponse<TestCase>(testCases);
        }
    }
}