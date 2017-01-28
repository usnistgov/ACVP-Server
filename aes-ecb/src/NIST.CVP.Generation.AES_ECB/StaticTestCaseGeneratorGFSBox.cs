using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using NIST.CVP.Generation.AES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_ECB
{
    public class StaticTestCaseGeneratorGFSBox : IStaticTestCaseGenerator<TestGroup, TestCase>
    {
        public MultipleTestCaseGenerateResponse<TestCase> Generate(TestGroup testGroup)
        {
            List<AlgoArrayResponse> data = new List<AlgoArrayResponse>();

            switch (testGroup.KeyLength)
            {
                case 128:
                    data = KATData.GetGFSBox128BitKey();
                    break;
                case 192:
                    data = KATData.GetGFSBox192BitKey();
                    break;
                case 256:
                    data = KATData.GetGFSBox256BitKey();
                    break;
                default:
                    return new MultipleTestCaseGenerateResponse<TestCase>($"Invalid {nameof(testGroup.KeyLength)} of {testGroup.KeyLength}");
            }

            var testCases = data.Select(s => new TestCase()
            {
                Key = s.Key,
                PlainText = s.PlainText,
                CipherText = s.CipherText
            }).ToList();

            return new MultipleTestCaseGenerateResponse<TestCase>(testCases);
        }
    }
}