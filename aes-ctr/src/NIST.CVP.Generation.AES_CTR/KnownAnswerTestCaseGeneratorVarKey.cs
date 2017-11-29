using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CTR
{
    public class KnownAnswerTestCaseGeneratorVarKey : IKnownAnswerTestCaseGenerator<TestGroup, TestCase>
    {
        public MultipleTestCaseGenerateResponse<TestCase> Generate(TestGroup testGroup)
        {
            var data = KatDataCtr.GetVarKey(testGroup.KeyLength);

            var testCases = data.Select(s => new TestCase
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
