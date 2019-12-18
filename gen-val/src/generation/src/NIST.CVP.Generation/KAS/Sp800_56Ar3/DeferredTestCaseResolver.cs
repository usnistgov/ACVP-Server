using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar3;
using NIST.CVP.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.KAS.Sp800_56Ar3
{
    public class DeferredTestCaseResolver<TTestGroup, TTestCase, TKeyPair> : IDeferredTestCaseResolverAsync<TTestGroup, TTestCase, KeyAgreementResult>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase, TKeyPair>
        where TTestCase : TestCaseBase<TTestGroup, TTestCase, TKeyPair>
        where TKeyPair : IDsaKeyPair
    {
        private readonly IOracle _oracle;

        public DeferredTestCaseResolver(IOracle oracle)
        {
            _oracle = oracle;
        }
        
        public async Task<KeyAgreementResult> CompleteDeferredCryptoAsync(TTestGroup serverTestGroup, TTestCase serverTestCase, TTestCase iutTestCase)
        {
            try
            {
                var result = await _oracle.CompleteDeferredKasTestAsync(new KasAftDeferredParameters()
                    {
                        KasAlgorithm = serverTestGroup.KasAlgorithm,
                        L = serverTestGroup.L,
                        DomainParameters = serverTestGroup.DomainParameters,
                        KasScheme = serverTestGroup.Scheme,
                        KdfParameter = serverTestCase.KdfParameter,
                        MacParameter = new MacParameters(
                            serverTestGroup.MacConfiguration.MacType,
                            serverTestGroup.MacConfiguration.KeyLen,
                            serverTestGroup.MacConfiguration.MacLen),
                        IutGenerationRequirements = serverTestGroup.KeyNonceGenRequirementsIut,
                        ServerGenerationRequirements = serverTestGroup.KeyNonceGenRequirementsServer,
                        
                        DkmNonceIut = iutTestCase.DkmNonceIut,
                        DkmNonceServer = serverTestCase.DkmNonceServer,
                        
                        EphemeralNonceIut = iutTestCase.EphemeralNonceIut,
                        EphemeralNonceServer = serverTestCase.EphemeralNonceServer,
                        
                        PartyIdServer = serverTestGroup.ServerId,
                        PartyIdIut = serverTestGroup.IutId,
                        
                        EphemeralKeyServer = serverTestCase.EphemeralKeyServer,
                        StaticKeyServer = serverTestCase.StaticKeyServer,
                        
                        EphemeralKeyIut = iutTestCase.EphemeralKeyIut,
                        StaticKeyIut = iutTestCase.StaticKeyIut,
                    }
                );

                return result.KasResult;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new KeyAgreementResult(ex.Message);
            }
        }

        private static Logger Logger => LogManager.GetCurrentClassLogger();
    }
}