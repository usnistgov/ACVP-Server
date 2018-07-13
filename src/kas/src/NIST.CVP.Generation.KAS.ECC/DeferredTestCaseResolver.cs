using System;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Generation.Core;
using NLog;

namespace NIST.CVP.Generation.KAS.ECC
{
    public class DeferredTestCaseResolver : IDeferredTestCaseResolver<TestGroup, TestCase, KasResult>
    {
        private readonly IOracle _oracle;

        public DeferredTestCaseResolver(IOracle oracle)
        {
            _oracle = oracle;
        }

        public KasResult CompleteDeferredCrypto(TestGroup serverTestGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            try
            {
                var result = _oracle.CompleteDeferredKasTest(new KasAftDeferredParametersEcc()
                    {
                        Curve = serverTestGroup.Curve,
                        DkmNonceIut = iutTestCase.DkmNonceIut,
                        DkmNonceServer = serverTestCase.DkmNonceServer,
                        EccParameterSet = serverTestGroup.ParmSet,
                        EccScheme = serverTestGroup.Scheme,
                        EphemeralNonceIut = iutTestCase.EphemeralNonceIut,
                        EphemeralNonceServer = serverTestCase.EphemeralNonceServer,
                        EphemeralPrivateKeyServer = serverTestCase.EphemeralPrivateKeyServer,
                        EphemeralPublicKeyIutX = iutTestCase.EphemeralPublicKeyIutX,
                        EphemeralPublicKeyIutY = iutTestCase.EphemeralPublicKeyIutY,
                        EphemeralPublicKeyServerX = serverTestCase.EphemeralPublicKeyServerX,
                        EphemeralPublicKeyServerY = serverTestCase.EphemeralPublicKeyServerY,
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
                        StaticPrivateKeyServer = serverTestCase.StaticPrivateKeyServer,
                        StaticPublicKeyIutX = iutTestCase.StaticPublicKeyIutX,
                        StaticPublicKeyIutY = iutTestCase.StaticPublicKeyIutY,
                        StaticPublicKeyServerX = serverTestCase.StaticPublicKeyServerX,
                        StaticPublicKeyServerY = serverTestCase.StaticPublicKeyServerY
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