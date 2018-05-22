using System;
using NIST.CVP.Crypto.Common.KDF.Components.TLS;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.TLS
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _rand;
        private readonly ITlsKdf _algo;

        public int NumberOfTestCasesToGenerate { get; private set; } = 100;

        public TestCaseGenerator(IRandom800_90 rand, ITlsKdf algo)
        {
            _rand = rand;
            _algo = algo;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 20;
            }

            var testCase = new TestCase
            {
                PreMasterSecret = _rand.GetRandomBitString(group.PreMasterSecretLength),
                ClientHelloRandom = _rand.GetRandomBitString(256),
                ServerHelloRandom = _rand.GetRandomBitString(256),
                ClientRandom = _rand.GetRandomBitString(256),
                ServerRandom = _rand.GetRandomBitString(256),
            };

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            TlsKdfResult tlsResult = null;
            try
            {
                tlsResult = _algo.DeriveKey(testCase.PreMasterSecret, testCase.ClientHelloRandom,
                    testCase.ServerHelloRandom, testCase.ClientRandom, testCase.ServerRandom, group.KeyBlockLength);
                if (!tlsResult.Success)
                {
                    ThisLogger.Warn(tlsResult.ErrorMessage);
                    return new TestCaseGenerateResponse<TestGroup, TestCase>(tlsResult.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex.StackTrace);
                return new TestCaseGenerateResponse<TestGroup, TestCase>(ex.Message);
            }

            testCase.MasterSecret = tlsResult.MasterSecret;
            testCase.KeyBlock = tlsResult.DerivedKey;

            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        public Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
