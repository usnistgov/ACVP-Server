using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.Common.Symmetric.TDES.KATs;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.TDES_OFBI
{
    public class TestCaseGeneratorKat : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly List<AlgoArrayResponseWithIvs> _kats;
        private readonly Dictionary<string, List<AlgoArrayResponseWithIvs>> _katMapping =
            new Dictionary<string, List<AlgoArrayResponseWithIvs>>()
            {
                {"permutation", KatData.GetPermutationDataOFBI()},
                {"inversepermutation", KatData.GetInversePermutationDataOFBI()},
                {"substitutiontable", KatData.GetSubstitutionTableDataOFBI()},
                {"variablekey", KatData.GetVariableKeyDataOFBI()},
                {"variabletext", KatData.GetVariableTextDataOFBI()}
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
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample)
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
                IV1 = currentKat.IV1,
                IV2 = currentKat.IV2,
                IV3 = currentKat.IV3
            };

            return await Task.FromResult(new TestCaseGenerateResponse<TestGroup, TestCase>(testCase));
        }
    }
}
