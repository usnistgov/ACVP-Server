using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Math.Helpers;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.KAS_IFC.v1_0
{
    public class TestCaseGeneratorAft : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public TestCaseGeneratorAft(IOracle oracle)
        {
            _oracle = oracle;
        }

        public int NumberOfTestCasesToGenerate => 10;

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup @group, bool isSample, int caseNo = -1)
        {
            try
            {
                var result = await _oracle.GetKasAftTestIfcAsync(new KasAftParametersIfc()
                {
                    IsSample = isSample,
                    L = group.L,
                    Modulo = group.Modulo,
                    PublicExponent = group.PublicExponent,
                    Scheme = group.Scheme,
                    KasMode = group.KasMode,
                    KdfConfiguration = group.KdfConfiguration,
                    KtsConfiguration = group.KtsConfiguration,
                    MacConfiguration = group.MacConfiguration,
                    KeyGenerationMethod = group.KeyGenerationMethod,
                    IutKeyAgreementRole = group.KasRole,
                    KeyConfirmationDirection = group.KeyConfirmationDirection,
                    IutKeyConfirmationRole = group.KeyConfirmationRole,
                    IutPartyId = group.IutId,
                    ServerPartyId = group.ServerId
                });

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase()
                {
                    Deferred = true,
                    TestPassed = true,
                    ServerC = result.ServerC,
                    ServerK = result.ServerK,
                    ServerNonce = result.ServerNonce,
                    ServerKey = result.ServerKeyPair ?? new KeyPair() { PubKey = new PublicKey() },
                    IutKey = result.IutKeyPair ?? new KeyPair() { PubKey = new PublicKey() },
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>(ex.Message);
            }
        }

        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    }
}