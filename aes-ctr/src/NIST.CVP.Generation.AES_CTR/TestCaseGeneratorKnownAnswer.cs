using System;
using System.Collections.Generic;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.AES_CTR;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_CTR
{
    public class TestCaseGeneratorKnownAnswer : ITestCaseGenerator<TestGroup, TestCase>
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