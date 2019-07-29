using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES.KATs;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.AES_CTR.v1_0
{
    public class TestCaseGeneratorKnownAnswer : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly List<AlgoArrayResponse> _kats = new List<AlgoArrayResponse>();
        private readonly Dictionary<(int keyLength, string katType), List<AlgoArrayResponse>> _katMapping =
            new Dictionary<(int keyLength, string katType), List<AlgoArrayResponse>>()
            {
                {(128, "gfsbox"), KatDataCtr.GetGfSBox(128)},
                {(192, "gfsbox"), KatDataCtr.GetGfSBox(192)},
                {(256, "gfsbox"), KatDataCtr.GetGfSBox(256)},
                {(128, "keysbox"), KatDataCtr.GetKeySBox(128)},
                {(192, "keysbox"), KatDataCtr.GetKeySBox(192)},
                {(256, "keysbox"), KatDataCtr.GetKeySBox(256)},
                {(128, "vartxt"), KatDataCtr.GetVarTxt(128)},
                {(192, "vartxt"), KatDataCtr.GetVarTxt(192)},
                {(256, "vartxt"), KatDataCtr.GetVarTxt(256)},
                {(128, "varkey"), KatDataCtr.GetVarKey(128)},
                {(192, "varkey"), KatDataCtr.GetVarKey(192)},
                {(256, "varkey"), KatDataCtr.GetVarKey(256)},
            };

        private int _katsIndex = 0;

        public TestCaseGeneratorKnownAnswer(int keyLength, string katType)
        {
            if (!_katMapping
                .TryFirst(w => w.Key.keyLength == keyLength &&
                               w.Key.katType.Equals(katType, StringComparison.OrdinalIgnoreCase),
                    out var result)
            )
            {
                throw new ArgumentException($"Invalid {nameof(keyLength)} and {nameof(katType)} combination");
            }

            _kats = result.Value;
        }

        public int NumberOfTestCasesToGenerate => _kats.Count;

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            if (_katsIndex + 1 > _kats.Count)
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>("No additional KATs exist.");
            }

            var currentKat = _kats[_katsIndex++];
            var testCase = new TestCase
            {
                Key = currentKat.Key,
                IV = currentKat.IV,
                PlainText = currentKat.PlainText,
                CipherText = currentKat.CipherText
            };

            return await Task.FromResult(new TestCaseGenerateResponse<TestGroup, TestCase>(testCase));
        }
    }
}