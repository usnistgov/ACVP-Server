using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar3;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.Sp800_56Ar3
{
    public abstract class DeferredTestCaseResolverBase<TTestGroup, TTestCase, TDomainParameters, TKeyPair> : IDeferredTestCaseResolverAsync<TTestGroup, TTestCase, KeyAgreementResult>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase, TKeyPair>
        where TTestCase : TestCaseBase<TTestGroup, TTestCase, TKeyPair>
        where TDomainParameters : IDsaDomainParameters
        where TKeyPair : IDsaKeyPair
    {
        protected readonly IOracle Oracle;

        protected DeferredTestCaseResolverBase(IOracle oracle)
        {
            Oracle = oracle;
        }

        public async Task<KeyAgreementResult> CompleteDeferredCryptoAsync(TTestGroup serverTestGroup, TTestCase serverTestCase, TTestCase iutTestCase)
        {
            try
            {
                var domainParameters = await GetDomainParameters(serverTestGroup);
                var param = new KasAftDeferredParameters()
                {
                    KasAlgorithm = serverTestGroup.KasAlgorithm,
                    L = serverTestGroup.L,
                    DomainParameters = domainParameters,
                    KasScheme = serverTestGroup.Scheme,
                    KdfParameter = serverTestCase.KdfParameter,

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
                };

                if (serverTestGroup.MacConfiguration != null)
                {
                    param.MacParameter = new MacParameters(
                        serverTestGroup.MacConfiguration.MacType,
                        serverTestGroup.MacConfiguration.KeyLen,
                        serverTestGroup.MacConfiguration.MacLen);
                }

                var result = await Oracle.CompleteDeferredKasTestAsync(param);

                return result.KasResult;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new KeyAgreementResult(ex.Message);
            }
        }

        protected abstract Task<TDomainParameters> GetDomainParameters(TTestGroup serverTestGroup);

        private static Logger Logger => LogManager.GetCurrentClassLogger();
    }
}
