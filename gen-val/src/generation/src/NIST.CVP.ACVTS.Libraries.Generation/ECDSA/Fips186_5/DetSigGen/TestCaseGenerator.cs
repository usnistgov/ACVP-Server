using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.ECDSA.v1_0.SigGen;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.ECDSA.Fips186_5.DetSigGen
{
    public class TestCaseGenerator : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 10;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public GenerateResponse PrepareGenerator(TestGroup testGroup, bool isSample)
        {
            // For deterministic ECDSA, for each curve + hash alg combination, we create a test group that tests an IUT's
            // ability to correctly left-pad the octet string representation of the private key with 0-bits to a length
            // of 8 * ceil(nLen/8) bits. We do this only when isSample. As the keypair is a group level value, there is 
            // no need to have more than one test case for test groups of this type.
            if (testGroup.TestPaddingDInDetECDSAPerMsgSecretNumberGeneration)
                NumberOfTestCasesToGenerate = 1;
            
            return new GenerateResponse();
        }
        
        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var param = new EcdsaSignatureParameters
            {
                Curve = group.Curve,
                HashAlg = group.HashAlg,
                PreHashedMessage = group.ComponentTest,
                Key = group.KeyPair,
                IsMessageRandomized = group.IsMessageRandomized,
                NonceProviderType = NonceProviderTypes.Deterministic        // Always the case for DetECDSA
            };

            try
            {
                TestCase testCase = null;
                EcdsaSignatureResult result = null;
                if (isSample)
                {
                    result = await _oracle.GetEcdsaSignatureAsync(param);
                    testCase = new TestCase
                    {
                        Message = result.Message,
                        RandomValue = result.RandomValue,
                        RandomValueLen = result.RandomValue?.BitLength ?? 0,
                        K = result.K,
                        Signature = result.Signature
                    };

                }
                else
                {
                    result = await _oracle.GetDeferredEcdsaSignatureAsync(param);
                    testCase = new TestCase
                    {
                        Message = result.Message
                    };
                }

                return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Error generating case: {ex.Message}");
            }
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
