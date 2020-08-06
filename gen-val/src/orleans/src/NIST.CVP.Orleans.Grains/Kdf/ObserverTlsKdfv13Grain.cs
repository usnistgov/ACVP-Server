using System;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Crypto.Common.KDF.Components.TLS;
using NIST.CVP.Crypto.Common.KDF.Components.TLS.Enums;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Interfaces.Kdf;

namespace NIST.CVP.Orleans.Grains.Kdf
{
	public class ObserverTlsKdfv13Grain : ObservableOracleGrainBase<TlsKdfv13Result>, IObserverTlsKdfv13Grain
	{
		private readonly ITLsKdfFactory_v1_3 _tlsFactory;
		private readonly IEntropyProvider _entropyProvider;
		
		private TlsKdfv13Parameters _param;
		
		public ObserverTlsKdfv13Grain(
			LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler, 
			ITLsKdfFactory_v1_3 tlsFactory, 
			IEntropyProviderFactory entropyProviderFactory) 
			: base(nonOrleansScheduler)
		{
			_tlsFactory = tlsFactory;
			_entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
		}

		public async Task<bool> BeginWorkAsync(TlsKdfv13Parameters param)
		{
			_param = param;

			await BeginGrainWorkAsync();
			return await Task.FromResult(true);
		}
		
		protected override async Task DoWorkAsync()
		{
			try
			{
				var kdf = _tlsFactory.GetInstance(_param.HashAlg);

				var digestLengthBits = ShaAttributes.GetHashFunctionFromEnum(_param.HashAlg).OutputLen;
				
				var psk = new BitString(digestLengthBits);
				var dhe = new BitString(digestLengthBits);

				if (new[] {TlsModes1_3.PSK, TlsModes1_3.PSK_DHE}.Contains(_param.RunningMode))
				{
					psk = _entropyProvider.GetEntropy(_param.RandomLength);
				}

				if (new[] {TlsModes1_3.DHE, TlsModes1_3.PSK_DHE}.Contains(_param.RunningMode))
				{
					dhe = _entropyProvider.GetEntropy(_param.RandomLength);
				}
				
				var helloClientRandom = _entropyProvider.GetEntropy(_param.RandomLength);
				var helloServerRandom = _entropyProvider.GetEntropy(_param.RandomLength);
			
				var finishClientRandom = _entropyProvider.GetEntropy(_param.RandomLength);
				var finishServerRandom = _entropyProvider.GetEntropy(_param.RandomLength);

				var dkm = kdf.GetFullKdf(
					false, psk, dhe, 
					helloClientRandom, finishClientRandom, 
					helloServerRandom, finishServerRandom);

				await Notify(new TlsKdfv13Result()
				{
					Psk = psk,
					Dhe = dhe,
					
					HelloClientRandom = helloClientRandom,
					HelloServerRandom = helloServerRandom,
					
					FinishClientRandom = finishClientRandom,
					FinishServerRandom = finishServerRandom,
					
					DerivedKeyingMaterial = dkm,
				});
			}
			catch (Exception e)
			{
				await Throw(e);
			}
		}
	}
}