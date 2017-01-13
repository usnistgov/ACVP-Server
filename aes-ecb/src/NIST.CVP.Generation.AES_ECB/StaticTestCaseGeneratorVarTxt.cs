using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_ECB
{
    public class StaticTestCaseGeneratorVarTxt : IStaticTestCaseGenerator<TestGroup, IEnumerable<TestCase>>
    {
        public MultipleTestCaseGenerateResponse<IEnumerable<TestCase>> Generate(TestGroup testGroup)
        {
            List<AlgoArrayResponse> data = new List<AlgoArrayResponse>();

            switch (testGroup.KeyLength)
            {
                case 128:
                    data = KATData.GetVarTxt128BitKey();
                    break;
                case 192:
                    data = KATData.GetVarTxt192BitKey();
                    break;
                case 256:
                    data = KATData.GetVarTxt256BitKey();
                    break;
                default:
                    return new MultipleTestCaseGenerateResponse<IEnumerable<TestCase>>($"Invalid {nameof(testGroup.KeyLength)} of {testGroup.KeyLength}");
            }

            var testCases = data.Select(s => new TestCase()
            {
                Key = s.Key,
                PlainText = s.PlainText,
                CipherText = s.CipherText
            }).ToList();

            return new MultipleTestCaseGenerateResponse<IEnumerable<TestCase>>(testCases);
        }
    }
}