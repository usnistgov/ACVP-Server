using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;
using System;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.DSA.v1_0.SigVer
{
    public class TestCaseGenerator : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 15;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public GenerateResponse PrepareGenerator(TestGroup @group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 5;
            }
            return new GenerateResponse();
        }
        
        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var keyParam = new DsaKeyParameters
            {
                DomainParameters = group.DomainParams
            };

            DsaKeyResult keyResult = null;
            try
            {
                keyResult = await _oracle.GetDsaKeyAsync(keyParam);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>("Unable to generate key");
            }

            ITestCaseExpectationReason<DsaSignatureDisposition> reason = group.TestCaseExpectationProvider.GetRandomReason();

            var param = new DsaSignatureParameters
            {
                HashAlg = group.HashAlg,
                DomainParameters = group.DomainParams,
                MessageLength = group.N,
                Key = keyResult.Key,
                Disposition = reason.GetReason(),
                IsMessageRandomized = group.IsMessageRandomized
            };

            try
            {
                var result = await _oracle.GetDsaSignatureAsync(param);

                var testCase = new TestCase
                {
                    Signature = result.Signature,
                    Message = result.Message,
                    RandomValue = result.RandomValue,
                    RandomValueLen = result.RandomValue?.BitLength ?? 0,
                    Key = result.Key,
                    Reason = reason,
                    TestPassed = reason.GetReason() == DsaSignatureDisposition.None
                };

                return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>("Unable to generate signature");
            }
        }

        private ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
