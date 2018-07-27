using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core;
using NLog;
using System;

namespace NIST.CVP.Generation.SRTP
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 10;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 2;
            }

            var param = new SrtpKdfParameters
            {
                AesKeyLength = group.AesKeyLength,
                KeyDerivationRate = group.Kdr
            };

            SrtpKdfResult result = null;
            try
            {
                result = _oracle.GetSrtpKdfCase(param);
            }
            catch (Exception ex)
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }

            var testCase = new TestCase
            {
                Index = result.Index,
                MasterKey = result.MasterKey,
                MasterSalt = result.MasterSalt,
                SrtcpIndex = result.SrtcpIndex,
                SrtcpKa = result.SrtcpAuthenticationKey,
                SrtcpKe = result.SrtcpEncryptionKey,
                SrtcpKs = result.SrtcpSaltingKey,
                SrtpKa = result.SrtpAuthenticationKey,
                SrtpKe = result.SrtpEncryptionKey,
                SrtpKs = result.SrtpSaltingKey
            };

            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            return null;
        }

        public Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
