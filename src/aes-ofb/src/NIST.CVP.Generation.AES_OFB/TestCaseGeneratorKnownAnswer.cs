using System;
using System.Collections.Generic;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.AES_OFB;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_OFB
{
    public class TestCaseGeneratorKnownAnswer : ITestCaseGenerator<TestGroup, TestCase>
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