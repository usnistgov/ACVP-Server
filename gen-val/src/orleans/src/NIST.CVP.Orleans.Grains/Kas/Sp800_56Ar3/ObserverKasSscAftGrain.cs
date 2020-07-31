using System;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar3;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar3;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3.Builders;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3.Enums;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Interfaces.Kas.Sp800_56Ar3;
using NIST.CVP.Orleans.Grains.Kas.Sp800_56Ar3.Helpers;

namespace NIST.CVP.Orleans.Grains.Kas.Sp800_56Ar3
{
	public class ObserverKasSscAftGrain : ObservableOracleGrainBase<KasSscAftResult>, 
        IObserverKasSscAftGrain
    {
        private readonly ISecretKeyingMaterialBuilder _secretKeyingMaterialBuilder;
        private readonly IDsaEccFactory _dsaEccFactory;
        private readonly IDsaFfcFactory _dsaFfcFactory;
        private readonly IEntropyProvider _entropyProvider;
        
        private KasSscAftParameters _param;
        
        public ObserverKasSscAftGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            ISecretKeyingMaterialBuilder secretKeyingMaterialBuilder,
            IDsaEccFactory dsaEccFactory,
            IDsaFfcFactory dsaFfcFactory,
            IEntropyProvider entropyProvider
        ) : base (nonOrleansScheduler)
        {
            _secretKeyingMaterialBuilder = secretKeyingMaterialBuilder;
            _dsaEccFactory = dsaEccFactory;
            _dsaFfcFactory = dsaFfcFactory;
            _entropyProvider = entropyProvider;
        }
        
        public async Task<bool> BeginWorkAsync(KasSscAftParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            var result = new KasSscAftResult();
        
            try
            {
                // TODO this is yucky... may need to bring in some generics around DomainParameters and KeyPair.
                switch (_param.KasAlgorithm)
                {
                    case KasAlgorithm.Ecc:
                        KeyingMaterialHelpers.SetSecretKeyingMaterialBuilderInformation(
                            _secretKeyingMaterialBuilder, 
                            _param.ServerGenerationRequirements,
                            (EccDomainParameters) _param.DomainParameters, 
                            _param.ServerEphemeralKey, _param.ServerStaticKey,
                            _entropyProvider,
                            null);
                        break;
                    case KasAlgorithm.Ffc:
                        KeyingMaterialHelpers.SetSecretKeyingMaterialBuilderInformation(
                            _secretKeyingMaterialBuilder, 
                            _param.ServerGenerationRequirements,
                            (FfcDomainParameters) _param.DomainParameters, 
                            _param.ServerEphemeralKey, _param.ServerStaticKey,
                            _entropyProvider,
                            null);
                        break;
                }

                result.ServerSecretKeyingMaterial = _secretKeyingMaterialBuilder.Build(
                    _param.KasScheme,
                    _param.ServerGenerationRequirements.KasMode, _param.ServerGenerationRequirements.ThisPartyKasRole,
                    _param.ServerGenerationRequirements.ThisPartyKeyConfirmationRole,
                    _param.ServerGenerationRequirements.KeyConfirmationDirection);
                
                await Notify(result);
            }
            catch (Exception ex)
            {
                await Throw(ex);
            }
        }
    }
}