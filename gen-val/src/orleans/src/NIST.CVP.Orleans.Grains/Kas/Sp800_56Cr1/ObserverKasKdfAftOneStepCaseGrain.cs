using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Cr1;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Cr1;
using NIST.CVP.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Interfaces.Kas.Sp800_56Cr1;

namespace NIST.CVP.Orleans.Grains.Kas.Sp800_56Cr1
{
	public class ObserverKasKdfAftOneStepCaseGrain : ObservableOracleGrainBase<KasKdfAftOneStepResult>, IObserverKasKdfAftOneStepCaseGrain
	{
		private const int LengthZ = 512;
		private const int LengthPartyId = 128;
		private const int LengthEphemeralData = 512;
		
		private readonly IKdfParameterVisitor _kdfParameterVisitor;
		private readonly IKdfVisitor _kdfVisitor;
		private readonly IEntropyProvider _entropyProvider;
		private readonly IFixedInfoFactory _fixedInfoFactory;

		private KasKdfAftOneStepParameters _param;
		

		public ObserverKasKdfAftOneStepCaseGrain(
			LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler, 
			IKdfParameterVisitor kdfParameterVisitor, 
			IKdfVisitor kdfVisitor,
			IEntropyProvider entropyProvider, 
			IFixedInfoFactory fixedInfoFactory) 
			: base(nonOrleansScheduler)
		{
			_kdfParameterVisitor = kdfParameterVisitor;
			_kdfVisitor = kdfVisitor;
			_entropyProvider = entropyProvider;
			_fixedInfoFactory = fixedInfoFactory;
		}

		public async Task<bool> BeginWorkAsync(KasKdfAftOneStepParameters param)
		{
			_param = param;

			await BeginGrainWorkAsync();
			return await Task.FromResult(true);
		}

		protected override async Task DoWorkAsync()
		{
			var kdfParam = _kdfParameterVisitor.CreateParameter(_param.OneStepConfiguration);
			kdfParam.Z = _entropyProvider.GetEntropy(LengthZ);

			var fixedInfoPartyU =
				new PartyFixedInfo(
					_entropyProvider.GetEntropy(LengthPartyId), 
					IncludeEphemeralData() ? _entropyProvider.GetEntropy(LengthEphemeralData) : null);
			var fixedInfoPartyV =
				new PartyFixedInfo(
					_entropyProvider.GetEntropy(LengthPartyId), 
					IncludeEphemeralData() ? _entropyProvider.GetEntropy(LengthEphemeralData) : null);
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

			await Notify(new KasKdfAftOneStepResult()
			{
				KdfInputs = (KdfParameterOneStep)kdfParam,
				FixedInfoPartyU = fixedInfoPartyU,
				FixedInfoPartyV = fixedInfoPartyV,
				DerivedKeyingMaterial = result.DerivedKey
			});
		}

		private bool IncludeEphemeralData()
		{
			return _entropyProvider.GetEntropy(1).Bits[0];
		}
	}
}