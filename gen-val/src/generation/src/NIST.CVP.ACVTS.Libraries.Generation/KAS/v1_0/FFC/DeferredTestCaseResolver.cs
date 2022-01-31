using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar1;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0.FFC
{
    public class DeferredTestCaseResolver : IDeferredTestCaseResolverAsync<TestGroup, TestCase, KasResult>
    {
        private readonly IOracle _oracle;

        public DeferredTestCaseResolver(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<KasResult> CompleteDeferredCryptoAsync(TestGroup serverTestGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            try
            {
                var result = await _oracle.CompleteDeferredKasTestAsync(new KasAftDeferredParametersFfc()
                {
                    P = serverTestGroup.DomainParams.P,
                    Q = serverTestGroup.DomainParams.Q,
                    G = serverTestGroup.DomainParams.G,
                    DkmNonceIut = iutTestCase.DkmNonceIut,
                    DkmNonceServer = serverTestCase.DkmNonceServer,
                    FfcParameterSet = serverTestGroup.ParmSet,
                    FfcScheme = serverTestGroup.Scheme,
                    EphemeralNonceIut = iutTestCase.EphemeralNonceIut,
                    EphemeralNonceServer = serverTestCase.EphemeralNonceServer,
                    EphemeralPrivateKeyServer = serverTestCase.EphemeralKeyServer.PrivateKeyX,
                    EphemeralPublicKeyIut = iutTestCase.EphemeralKeyIut.PublicKeyY,
                    EphemeralPublicKeyServer = serverTestCase.EphemeralKeyServer.PublicKeyY,
                    HashFunction = serverTestGroup.HashAlg,
                    IdIut = iutTestCase.IdIut ?? serverTestGroup.IdIut,
                    IdServer = serverTestGroup.IdServer,
                    IutKeyAgreementRole = serverTestGroup.KasRole,
                    IutKeyConfirmationRole = serverTestGroup.KcRole,
                    KasMode = serverTestGroup.KasMode,
                    KeyConfirmationDirection = serverTestGroup.KcType,
                    KeyLen = serverTestGroup.KeyLen,
                    MacLen = serverTestGroup.MacLen,
                    MacType = serverTestGroup.MacType,
                    NonceAesCcm = iutTestCase.NonceAesCcm ?? serverTestCase.NonceAesCcm,
                    NonceNoKc = serverTestCase.NonceNoKc,
                    OiLen = iutTestCase.OiLen,
                    OiPattern = serverTestGroup.OiPattern,
                    OtherInfo = iutTestCase.OtherInfo,
                    StaticPrivateKeyServer = serverTestCase.StaticKeyServer.PrivateKeyX,
                    StaticPublicKeyIut = iutTestCase.StaticKeyIut.PublicKeyY,
                    StaticPublicKeyServer = serverTestCase.StaticKeyServer.PublicKeyY
                }
                );

                var kasResult = result.Result;

                return new KasResult(
                    kasResult.Z, kasResult.Oi, kasResult.Dkm, kasResult.MacData, kasResult.Tag
                );
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new KasResult(ex.Message);
            }
        }

        private static Logger Logger => LogManager.GetCurrentClassLogger();
    }
}
