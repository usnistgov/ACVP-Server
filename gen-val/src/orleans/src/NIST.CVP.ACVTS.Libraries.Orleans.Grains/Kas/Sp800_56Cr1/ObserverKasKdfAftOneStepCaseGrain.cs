using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Cr1;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Cr1;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Kas.Sp800_56Cr1;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Kas.Sp800_56Cr1
{
    public class ObserverKdaAftOneStepCaseGrain : ObservableOracleGrainBase<KdaAftOneStepResult>, IObserverKdaAftOneStepCaseGrain
    {
        private const int LengthPartyId = 128;

        private readonly IKdfParameterVisitor _kdfParameterVisitor;
        private readonly IKdfVisitor _kdfVisitor;
        private readonly IEntropyProvider _entropyProvider;
        private readonly IFixedInfoFactory _fixedInfoFactory;

        private KdaAftOneStepParameters _param;


        public ObserverKdaAftOneStepCaseGrain(
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

        public async Task<bool> BeginWorkAsync(KdaAftOneStepParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            try
            {
                var kdfParam = _kdfParameterVisitor.CreateParameter(_param.OneStepConfiguration);
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
                    T = kdfParam.T,
                    FixedInfoPattern = kdfParam.FixedInfoPattern,
                    EntropyBits = kdfParam.EntropyBits,
                };
                fixedInfoParam.SetFixedInfo(fixedInfoPartyU, fixedInfoPartyV);

                var fixedInfo = _fixedInfoFactory.Get().Get(fixedInfoParam);

                var result = kdfParam.AcceptKdf(_kdfVisitor, fixedInfo);

                await Notify(new KdaAftOneStepResult()
                {
                    KdfInputs = (KdfParameterOneStep)kdfParam,
                    FixedInfoPartyU = fixedInfoPartyU,
                    FixedInfoPartyV = fixedInfoPartyV,
                    DerivedKeyingMaterial = result.DerivedKey
                });
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
