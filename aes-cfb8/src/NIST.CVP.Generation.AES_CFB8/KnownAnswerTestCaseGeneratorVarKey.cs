using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_CFB8
{
    public class KnownAnswerTestCaseGeneratorVarKey : IKnownAnswerTestCaseGenerator<TestGroup, TestCase>
    {
        public MultipleTestCaseGenerateResponse<TestCase> Generate(TestGroup testGroup)
        {
            List<AlgoArrayResponse> data = new List<AlgoArrayResponse>();

            switch (testGroup.KeyLength)
            {
                case 128:
                    data = KATDataCFB8.GetVarKey128BitKey();
                    break;
                case 192:
                    data = KATDataCFB8.GetVarKey192BitKey();
                    break;
                case 256:
                    data = KATDataCFB8.GetVarKey256BitKey();
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