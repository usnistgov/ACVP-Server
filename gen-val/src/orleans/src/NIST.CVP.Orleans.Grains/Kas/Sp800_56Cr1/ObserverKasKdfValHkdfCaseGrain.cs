using System;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Cr1;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Cr1;
using NIST.CVP.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfHkdf;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Interfaces.Kas.Sp800_56Cr1;

namespace NIST.CVP.Orleans.Grains.Kas.Sp800_56Cr1
{
	public class ObserverKasKdfValHkdfCaseGrain : ObservableOracleGrainBase<KasKdfValHkdfResult>, IObserverKasKdfValHkdfCaseGrain
	{
		private const int LengthPartyId = 128;
		
		private readonly IKdfParameterVisitor _kdfParameterVisitor;
		private readonly IKdfVisitor _kdfVisitor;
		private readonly IEntropyProvider _entropyProvider;
		private readonly IRandom800_90 _random;
		private readonly IFixedInfoFactory _fixedInfoFactory;

		private KasKdfValHkdfParameters _param;
		

		public ObserverKasKdfValHkdfCaseGrain(
			LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler, 
			IKdfParameterVisitor kdfParameterVisitor, 
			IKdfVisitor kdfVisitor,
			IEntropyProvider entropyProvider, 
			IRandom800_90 random,
			IFixedInfoFactory fixedInfoFactory) 
			: base(nonOrleansScheduler)
		{
			_kdfParameterVisitor = kdfParameterVisitor;
			_kdfVisitor = kdfVisitor;
			_entropyProvider = entropyProvider;
			_random = random;
			_fixedInfoFactory = fixedInfoFactory;
		}

		public async Task<bool> BeginWorkAsync(KasKdfValHkdfParameters param)
		{
			_param = param;

			await BeginGrainWorkAsync();
			return await Task.FromResult(true);
		}

		protected override async Task DoWorkAsync()
		{
			try
			{
				var testPassed = true;
				
				while (true)
				{
					var kdfParam = _kdfParameterVisitor.CreateParameter(_param.KdfConfiguration);
					kdfParam.Z = _entropyProvider.GetEntropy(_param.ZLength);

					var fixedInfoPartyU =
						new PartyFixedInfo(
							_entropyProvider.GetEntropy(LengthPartyId), 
							IncludeEphemeralData() ? _entropyProvider.GetEntropy(_param.ZLength) : null);
					var fixedInfoPartyV =
						new PartyFixedInfo(
							_entropyProvider.GetEntropy(LengthPartyId), 
							IncludeEphemeralData() ? _entropyProvider.GetEntropy(_param.ZLength) : null);
					var fixedInfoParam = new FixedInfoParameter()
					{
						Context = kdfParam.Context,
						Encoding = kdfParam.FixedInputEncoding,
						Iv = kdfParam.Iv,
						L = kdfParam.L,
						Label = kdfParam.Label,
						Salt = kdfParam.Salt,
						AlgorithmId = kdfParam.AlgorithmId,
						FixedInfoPattern = kdfParam.FixedInfoPattern,
					};
					fixedInfoParam.SetFixedInfo(fixedInfoPartyU, fixedInfoPartyV);
			
					var fixedInfo = _fixedInfoFactory.Get().Get(fixedInfoParam);

					var result = kdfParam.AcceptKdf(_kdfVisitor, fixedInfo);

					switch (_param.Disposition)
					{
						case KasKdfTestCaseDisposition.SuccessLeadingZeroNibble when result.DerivedKey[0] >= 0x10:
							continue;
						case KasKdfTestCaseDisposition.Fail:
							kdfParam.Z = _random.GetDifferentBitStringOfSameSize(kdfParam.Z);
							testPassed = false;
							break;
					}

					await Notify(new KasKdfValHkdfResult()
					{
						KdfInputs = (KdfParameterHkdf)kdfParam,
						FixedInfoPartyU = fixedInfoPartyU,
						FixedInfoPartyV = fixedInfoPartyV,
						DerivedKeyingMaterial = result.DerivedKey,
						TestPassed = testPassed
					});
					break;
				}
			}
			catch (Exception e)
			{
				await Throw(e);
			}
		}

		private bool IncludeEphemeralData()
		{
			return _entropyProvider.GetEntropy(1).Bits[0];
		}
	}
}