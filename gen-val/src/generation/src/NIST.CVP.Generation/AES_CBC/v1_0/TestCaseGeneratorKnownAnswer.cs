﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES.KATs;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_CBC.v1_0
{
    public class TestCaseGeneratorKnownAnswer : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly List<AlgoArrayResponse> _kats = new List<AlgoArrayResponse>();
        private readonly Dictionary<(int keyLength, string katType), List<AlgoArrayResponse>> _katMapping =
            new Dictionary<(int keyLength, string katType), List<AlgoArrayResponse>>()
            {
                {(128, "gfsbox"), KATData.GetGFSBox128BitKey()},
                {(192, "gfsbox"), KATData.GetGFSBox192BitKey()},
                {(256, "gfsbox"), KATData.GetGFSBox256BitKey()},
                {(128, "keysbox"), KATData.GetKeySBox128BitKey()},
                {(192, "keysbox"), KATData.GetKeySBox192BitKey()},
                {(256, "keysbox"), KATData.GetKeySBox256BitKey()},
                {(128, "vartxt"), KATData.GetVarTxt128BitKey()},
                {(192, "vartxt"), KATData.GetVarTxt192BitKey()},
                {(256, "vartxt"), KATData.GetVarTxt256BitKey()},
                {(128, "varkey"), KATData.GetVarKey128BitKey()},
                {(192, "varkey"), KATData.GetVarKey192BitKey()},
                {(256, "varkey"), KATData.GetVarKey256BitKey()},
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
            _kats.ForEach(fe => fe.IV = new BitString(128));
        }

        public int NumberOfTestCasesToGenerate => _kats.Count;

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            if (_katsIndex + 1 > _kats.Count)
            {
                return await Task.FromResult(new TestCaseGenerateResponse<TestGroup, TestCase>("No additional KATs exist."));
            }

            var currentKat = _kats[_katsIndex++];
            TestCase testCase = new TestCase
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