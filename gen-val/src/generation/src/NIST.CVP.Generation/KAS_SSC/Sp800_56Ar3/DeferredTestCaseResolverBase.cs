using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar3;
using NIST.CVP.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.KAS_SSC.Sp800_56Ar3
{
	public abstract class DeferredTestCaseResolverBase<TTestGroup, TTestCase, TDomainParameters, TKeyPair> : IDeferredTestCaseResolverAsync<TTestGroup, TTestCase, KeyAgreementResult>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase, TKeyPair>, new()
        where TTestCase : TestCaseBase<TTestGroup, TTestCase, TKeyPair>, new()
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
                var param = new KasSscAftDeferredParameters()
                {
                    DomainParameters = domainParameters,
                    KasScheme = serverTestGroup.Scheme,
                    
                    IutGenerationRequirements = serverTestGroup.KeyNonceGenRequirementsIut,
                    ServerGenerationRequirements = serverTestGroup.KeyNonceGenRequirementsServer,

                    EphemeralKeyServer = serverTestCase.EphemeralKeyServer,
                    StaticKeyServer = serverTestCase.StaticKeyServer,

                    EphemeralKeyIut = iutTestCase.EphemeralKeyIut,
                    StaticKeyIut = iutTestCase.StaticKeyIut,
                    
                    HashFunctionZ = serverTestGroup.HashFunctionZ
                };

                var result = await Oracle.CompleteDeferredKasSscAftTestAsync(param);

                return result.KasResult;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new KeyAgreementResult(ex.Message);
            }
        }

        protected abstract Task<TDomainParameters> GetDomainParameters(TTestGroup serverTestGroup);

        private static Logger Logger = LogManager.GetCurrentClassLogger();
    }
}