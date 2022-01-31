using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.Fips186_5.SigGen
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
                NumberOfTestCasesToGenerate = 3;
            }
            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var param = new RsaSignatureParameters
            {
                HashAlg = group.HashAlg,
                Key = group.Key,
                Modulo = group.Modulo,
                PaddingScheme = group.Mode,
                SaltLength = group.SaltLen,
                MaskFunction = group.MaskFunction,
                IsMessageRandomized = group.IsMessageRandomized
            };

            try
            {
                RsaSignatureResult result = null;
                if (isSample)
                {
                    result = await _oracle.GetRsaSignatureAsync(param);
                }
                else
                {
                    result = await _oracle.GetDeferredRsaSignatureAsync(param);
                }

                var testCase = new TestCase
                {
                    Message = result.Message,
                    RandomValue = result.RandomValue,
                    RandomValueLen = result.RandomValue?.BitLength ?? 0,
                    Signature = result.Signature?.PadToModulusMsb(group.Modulo),
                    Salt = result.Salt,
                    Deferred = !isSample
                };

                return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }

        private ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
