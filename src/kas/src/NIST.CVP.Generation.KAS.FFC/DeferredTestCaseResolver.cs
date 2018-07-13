using System;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Entropy;
using NLog;

namespace NIST.CVP.Generation.KAS.FFC
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
                var result = _oracle.CompleteDeferredKasTest(new KasAftDeferredParametersFfc()
                    {
                        P = serverTestGroup.P,
                        Q = serverTestGroup.Q,
                        G = serverTestGroup.G,
                        DkmNonceIut = iutTestCase.DkmNonceIut,
                        DkmNonceServer = serverTestCase.DkmNonceServer,
                        FfcParameterSet = serverTestGroup.ParmSet,
                        FfcScheme = serverTestGroup.Scheme,
                        EphemeralNonceIut = iutTestCase.EphemeralNonceIut,
                        EphemeralNonceServer = serverTestCase.EphemeralNonceServer,
                        EphemeralPrivateKeyServer = serverTestCase.EphemeralPrivateKeyServer,
                        EphemeralPublicKeyIut = iutTestCase.EphemeralPublicKeyIut,
                        EphemeralPublicKeyServer = serverTestCase.EphemeralPublicKeyServer,
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
                        StaticPublicKeyIut = iutTestCase.StaticPublicKeyIut,
                        StaticPublicKeyServer = serverTestCase.StaticPublicKeyServer
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