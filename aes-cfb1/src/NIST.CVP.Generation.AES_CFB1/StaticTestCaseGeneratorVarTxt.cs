using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.AES;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CFB1
{
    public class StaticTestCaseGeneratorVarTxt : IStaticTestCaseGenerator<TestGroup, TestCase>
    {
        public MultipleTestCaseGenerateResponse<TestCase> Generate(TestGroup testGroup)
        {
            List<AlgoArrayResponse> data = new List<AlgoArrayResponse>();

            switch (testGroup.KeyLength)
            {
                case 128:
                    data = KATDataCFB1.GetVarTxt128BitKey();
                    break;
                case 192:
                    data = KATDataCFB1.GetVarTxt192BitKey();
                    break;
                case 256:
                    data = KATDataCFB1.GetVarTxt256BitKey();
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