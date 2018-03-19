using System;
using NIST.CVP.Crypto.Common.KDF.Components.SRTP;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.SRTP
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _rand;
        private readonly ISrtp _algo;

        private const int SALT_LENGTH = 112;
        private const int INDEX_LENGTH = 48;
        private const int SRTCP_INDEX_LENGTH = 32;

        public int NumberOfTestCasesToGenerate { get; private set; } = 10;

        public TestCaseGenerator(IRandom800_90 rand, ISrtp algo)
        {
            _rand = rand;
            _algo = algo;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 2;
            }

            var key = _rand.GetRandomBitString(group.AesKeyLength);
            var salt = _rand.GetRandomBitString(SALT_LENGTH);
            var index = _rand.GetRandomBitString(INDEX_LENGTH);
            var srtcpIndex = _rand.GetRandomBitString(SRTCP_INDEX_LENGTH);

            var testCase = new TestCase
            {
                MasterKey = key,
                MasterSalt = salt,
                Index = index,
                SrtcpIndex = srtcpIndex
            };

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            KdfResult kdfResult = null;
            try
            {
                kdfResult = _algo.DeriveKey(group.AesKeyLength, testCase.MasterKey, testCase.MasterSalt, group.Kdr, testCase.Index, testCase.SrtcpIndex);
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

            testCase.SrtpKe = kdfResult.SrtpResult.EncryptionKey;
            testCase.SrtpKa = kdfResult.SrtpResult.AuthenticationKey;
            testCase.SrtpKs = kdfResult.SrtpResult.SaltingKey;

            testCase.SrtcpKe = kdfResult.SrtcpResult.EncryptionKey;
            testCase.SrtcpKa = kdfResult.SrtcpResult.AuthenticationKey;
            testCase.SrtcpKs = kdfResult.SrtcpResult.SaltingKey;

            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        public Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
