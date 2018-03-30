using System;
using System.Collections.Generic;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.AES_CFB8;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_CFB8
{
    public class TestCaseGeneratorKnownAnswer : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly List<AlgoArrayResponse> _kats = new List<AlgoArrayResponse>();
        private readonly Dictionary<(int keyLength, string katType), List<AlgoArrayResponse>> _katMapping =
            new Dictionary<(int keyLength, string katType), List<AlgoArrayResponse>>()
            {
                {(128, "gfsbox"), KATDataCFB8.GetGFSBox128BitKey()},
                {(192, "gfsbox"), KATDataCFB8.GetGFSBox192BitKey()},
                {(256, "gfsbox"), KATDataCFB8.GetGFSBox256BitKey()},
                {(128, "keysbox"), KATDataCFB8.GetKeySBox128BitKey()},
                {(192, "keysbox"), KATDataCFB8.GetKeySBox192BitKey()},
                {(256, "keysbox"), KATDataCFB8.GetKeySBox256BitKey()},
                {(128, "vartxt"), KATDataCFB8.GetVarTxt128BitKey()},
                {(192, "vartxt"), KATDataCFB8.GetVarTxt192BitKey()},
                {(256, "vartxt"), KATDataCFB8.GetVarTxt256BitKey()},
                {(128, "varkey"), KATDataCFB8.GetVarKey128BitKey()},
                {(192, "varkey"), KATDataCFB8.GetVarKey192BitKey()},
                {(256, "varkey"), KATDataCFB8.GetVarKey256BitKey()},
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

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            TestCase testCase = new TestCase();
            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
            if (_katsIndex + 1 > _kats.Count)
            {
                return new TestCaseGenerateResponse("No additional KATs exist.");
            }

            var currentKat = _kats[_katsIndex++];
            testCase.Key = currentKat.Key;
            testCase.IV = currentKat.IV;
            testCase.PlainText = currentKat.PlainText;
            testCase.CipherText = currentKat.CipherText;

            return new TestCaseGenerateResponse(testCase);
        }
    }
}