using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES.KATs;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.AES_OFB.v1_0
{
    public class TestCaseGeneratorKnownAnswer : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly List<AlgoArrayResponse> _kats = new List<AlgoArrayResponse>();
        private readonly Dictionary<(int keyLength, string katType), List<AlgoArrayResponse>> _katMapping =
            new Dictionary<(int keyLength, string katType), List<AlgoArrayResponse>>()
            {
                {(128, "gfsbox"), KATDataOFB.GetGFSBox128BitKey()},
                {(192, "gfsbox"), KATDataOFB.GetGFSBox192BitKey()},
                {(256, "gfsbox"), KATDataOFB.GetGFSBox256BitKey()},
                {(128, "keysbox"), KATDataOFB.GetKeySBox128BitKey()},
                {(192, "keysbox"), KATDataOFB.GetKeySBox192BitKey()},
                {(256, "keysbox"), KATDataOFB.GetKeySBox256BitKey()},
                {(128, "vartxt"), KATDataOFB.GetVarTxt128BitKey()},
                {(192, "vartxt"), KATDataOFB.GetVarTxt192BitKey()},
                {(256, "vartxt"), KATDataOFB.GetVarTxt256BitKey()},
                {(128, "varkey"), KATDataOFB.GetVarKey128BitKey()},
                {(192, "varkey"), KATDataOFB.GetVarKey192BitKey()},
                {(256, "varkey"), KATDataOFB.GetVarKey256BitKey()},
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

        public Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample)
        {
            if (_katsIndex + 1 > _kats.Count)
            {
                return Task.FromResult(new TestCaseGenerateResponse<TestGroup, TestCase>("No additional KATs exist."));
            }

            var currentKat = _kats[_katsIndex++];
            var testCase = new TestCase
            {
                Key = currentKat.Key,
                IV = currentKat.IV,
                PlainText = currentKat.PlainText,
                CipherText = currentKat.CipherText
            };

            return Task.FromResult(new TestCaseGenerateResponse<TestGroup, TestCase>(testCase));
        }
    }
}