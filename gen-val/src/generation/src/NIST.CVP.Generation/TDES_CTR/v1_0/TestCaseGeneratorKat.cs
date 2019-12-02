using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.Common.Symmetric.TDES.KATs;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.TDES_CTR.v1_0
{
    public class TestCaseGeneratorKat : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly List<AlgoArrayResponse> _kats;
        private readonly Dictionary<string, List<AlgoArrayResponse>> _katMapping =
            new Dictionary<string, List<AlgoArrayResponse>>()
            {
                {"permutation", KatData.GetPermutationData()},
                {"inversepermutation", KatData.GetInversePermutationData()},
                {"substitutiontable", KatData.GetSubstitutionTableData()},
                //{"variablekey", KatData.GetVariableKeyDataOFB()},        // See comment in OFB
                {"variablekey", KatData.GetVariableKeyData()},
                {"variabletext", KatData.GetVariableTextData()}
            };

        private int _katsIndex;

        public int NumberOfTestCasesToGenerate => _kats.Count;

        public TestCaseGeneratorKat(string katType)
        {
            if (!_katMapping
                .TryFirst(w => w.Key.Equals(katType, StringComparison.OrdinalIgnoreCase),
                    out var result)
            )
            {
                throw new ArgumentException($"Invalid {nameof(katType)}");
            }

            _kats = result.Value;
            _kats.ForEach(fe =>
            {
                fe.IV = fe.PlainText.GetDeepCopy();
                fe.PlainText = BitString.Zeroes(64);
            });
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            if (_katsIndex + 1 > _kats.Count)
            {
                return await Task.FromResult(new TestCaseGenerateResponse<TestGroup, TestCase>("No additional KATs exist."));
            }

            var currentKat = _kats[_katsIndex++];
            var testCase = new TestCase
            {
                Key1 = currentKat.Key1,
                Key2 = currentKat.Key2,
                Key3 = currentKat.Key3,
                PlainText = currentKat.PlainText,
                CipherText = currentKat.CipherText,
                Iv = currentKat.IV
            };

            return await Task.FromResult(new TestCaseGenerateResponse<TestGroup, TestCase>(testCase));
        }
    }
}
