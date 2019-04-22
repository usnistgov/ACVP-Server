using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES.KATs;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NLog;

namespace NIST.CVP.Generation.AES_CBC_CTS.v1_0
{
    public class TestCaseGeneratorKnownAnswerPartialBlock : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
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

        private bool _initialized = false;
        private int _chosenPayloadLen = 0;

        private int _katsIndex = 0;

        public TestCaseGeneratorKnownAnswerPartialBlock(IOracle oracle, int keyLength, string katType)
        {
            _oracle = oracle;

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

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample)
        {
            if (_katsIndex + 1 > _kats.Count)
            {
                return await Task.FromResult(new TestCaseGenerateResponse<TestGroup, TestCase>("No additional KATs exist."));
            }

            if (!_initialized)
            {
                _initialized = true;
                ChoosePayloadLen(group.PayloadLen);
            }

            var currentKat = _kats[_katsIndex++];
            TestCase testCase = new TestCase
            {
                Key = currentKat.Key,
                IV = currentKat.IV,
                PlainText = currentKat.PlainText
            };

            testCase.PlainText =
                testCase.PlainText.ConcatenateBits(new BitString(_chosenPayloadLen - currentKat.PlainText.BitLength));

            // Always invoke the engine in encrypt mode
            var param = new AesWithPayloadParameters
            {
                Mode = BlockCipherModesOfOperation.CbcCts,
                Payload = testCase.PlainText,
                Direction = "encrypt",
                KeyLength = group.KeyLength
            };

            try
            {
                var oracleResult = await _oracle.GetAesCaseAsync(param);
                testCase.CipherText = oracleResult.CipherText;

                return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }

        private void ChoosePayloadLen(MathDomain groupPayloadLen)
        {
            List<int> values = new List<int>();
            // Use larger numbers only when the "smaller" values don't exist.
            values.AddRangeIfNotNullOrEmpty(groupPayloadLen.GetValues(a => a > 128 && a < 1280 && a % 128 != 0, 128, true));
            values.AddRangeIfNotNullOrEmpty(groupPayloadLen.GetValues(a => a % 128 != 0, 128, true));

            _chosenPayloadLen = values.First();
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
