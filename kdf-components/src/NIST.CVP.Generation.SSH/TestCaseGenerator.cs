using System;
using NIST.CVP.Crypto.Common.KDF.Components.SSH;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.SSH
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _rand;
        private readonly ISsh _algo;
        private const int SHARED_SECRET_LENGTH = 2048;

        public int NumberOfTestCasesToGenerate { get; private set; } = 100;

        public TestCaseGenerator(IRandom800_90 rand, ISsh algo)
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

            // Get a random 2048-bit value (256-byte)
            var k = _rand.GetRandomBitString(SHARED_SECRET_LENGTH);

            // If the MSbit is a 1, append "00" to the front
            if (k.GetMostSignificantBits(1).Equals(BitString.One()))
            {
                k = BitString.Zeroes(8).ConcatenateBits(k);
            }

            // Append the length (32-bit) to the front (in bytes, so 256 or 257 bytes)
            var fullK = BitString.To32BitString(k.BitLength / 8).ConcatenateBits(k);
            var randH = _rand.GetRandomBitString(group.HashAlg.OutputLen);
            var randSessionId = _rand.GetRandomBitString(group.HashAlg.OutputLen);

            var testCase = new TestCase
            {
                K = fullK,
                H = randH,
                SessionId = randSessionId
            };

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            KdfResult kdfResult = null;
            try
            {
                kdfResult = _algo.DeriveKey(testCase.K, testCase.H, testCase.SessionId);
                if (!kdfResult.Success)
                {
                    ThisLogger.Warn(kdfResult.ErrorMessage);
                    return new TestCaseGenerateResponse<TestGroup, TestCase>(kdfResult.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex.StackTrace);
                return new TestCaseGenerateResponse<TestGroup, TestCase>(ex.Message);
            }

            testCase.InitialIvClient = kdfResult.ClientToServer.InitialIv;
            testCase.EncryptionKeyClient = kdfResult.ClientToServer.EncryptionKey;
            testCase.IntegrityKeyClient = kdfResult.ClientToServer.IntegrityKey;

            testCase.InitialIvServer = kdfResult.ServerToClient.InitialIv;
            testCase.EncryptionKeyServer = kdfResult.ServerToClient.EncryptionKey;
            testCase.IntegrityKeyServer = kdfResult.ServerToClient.IntegrityKey;

            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        public Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
