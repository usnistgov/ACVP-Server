using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.KDF_Components.v1_0.SRTP
{
    public class TestCaseGenerator : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 10;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public GenerateResponse PrepareGenerator(TestGroup @group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 2;
            }
            return new GenerateResponse();
        }
        
        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var param = new SrtpKdfParameters
            {
                AesKeyLength = group.AesKeyLength,
                KeyDerivationRate = group.Kdr
            };

            try
            {
                var result = await _oracle.GetSrtpKdfCaseAsync(param);

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
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }
        
        public ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
