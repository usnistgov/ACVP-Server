﻿using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.KAS.v1_0.FFC
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