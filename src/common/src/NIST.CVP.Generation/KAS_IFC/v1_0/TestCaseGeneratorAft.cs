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
    public class TestCaseGeneratorAft : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private List<KeyPair> _keys = new List<KeyPair>();

        public TestCaseGeneratorAft(IOracle oracle)
        {
            _oracle = oracle;
        }

        public int NumberOfTestCasesToGenerate => 10;

        public GenerateResponse PrepareGenerator(TestGroup @group, bool isSample)
        {
            // Get IUT keys matching the group's public exponent and modulo
            _keys = group.IutKeys
                .Where(w =>
                    w.E == group.PublicExponent &&
                    w.N.ExactBitLength().ValueToMod(1024) == group.Modulo &&
                    w.PrivateKeyFormat == group.KeyGenerationMethod)
                .Select(s => new KeyPair()
                {
                    PubKey = new PublicKey()
                    {
                        E = s.E,
                        N = s.N
                    }
                })
                .ToList();

            // Get IUT keys not taking into account the group public exponent (so random public exponent key groups)
            if (_keys.Count == 0)
            {
                _keys = group.IutKeys
                    .Where(w =>
                        w.N.ExactBitLength().ValueToMod(1024) == group.Modulo &&
                        w.PrivateKeyFormat == group.KeyGenerationMethod)
                    .Select(s => new KeyPair()
                    {
                        PubKey = new PublicKey()
                        {
                            E = s.E,
                            N = s.N
                        }
                    })
                    .ToList();
            }

            if (_keys.Count == 0)
            {
                return new GenerateResponse("Unable to associated provided IUT keys to this group information.");
            }

            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup @group, bool isSample, int caseNo = -1)
        {
            try
            {
                var iutKey = _keys[caseNo % _keys.Count];

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
                    IutKey = iutKey,
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