using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfTwoStep;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Cr1;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Cr1;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Kas.Sp800_56Cr1;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Kas.Sp800_56Cr1
{
    public class ObserverKdaAftTwoStepCaseGrain : ObservableOracleGrainBase<KdaAftTwoStepResult>, IObserverKdaAftTwoStepCaseGrain
    {
        private const int LengthPartyId = 128;

        private readonly IKdfParameterVisitor _kdfParameterVisitor;
        private readonly IKdfVisitor _kdfVisitor;
        private readonly IEntropyProvider _entropyProvider;
        private readonly IFixedInfoFactory _fixedInfoFactory;
        private readonly IKdfMultiExpansionParameterVisitor _kdfMultiExpansionParameterVisitor;
        private readonly IKdfMultiExpansionVisitor _kdfMultiExpansionVisitor;

        private KdaAftTwoStepParameters _param;

        public ObserverKdaAftTwoStepCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IKdfParameterVisitor kdfParameterVisitor,
            IKdfVisitor kdfVisitor,
            IEntropyProvider entropyProvider,
            IFixedInfoFactory fixedInfoFactory,
            IKdfMultiExpansionParameterVisitor kdfMultiExpansionParameterVisitor,
            IKdfMultiExpansionVisitor kdfMultiExpansionVisitor)
            : base(nonOrleansScheduler)
        {
            _kdfParameterVisitor = kdfParameterVisitor;
            _kdfVisitor = kdfVisitor;
            _entropyProvider = entropyProvider;
            _fixedInfoFactory = fixedInfoFactory;
            _kdfMultiExpansionParameterVisitor = kdfMultiExpansionParameterVisitor;
            _kdfMultiExpansionVisitor = kdfMultiExpansionVisitor;
        }

        public async Task<bool> BeginWorkAsync(KdaAftTwoStepParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            try
            {
                if (_param.KdfConfiguration != null)
                {
                    await Kdf();
                }
                else
                {
                    await KdfMultiExpand();
                }
            }
            catch (Exception e)
            {
                await Throw(e);
            }
        }

        private async Task Kdf()
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
                T = kdfParam.T,
                FixedInfoPattern = kdfParam.FixedInfoPattern,
                EntropyBits = kdfParam.EntropyBits,
            };
            fixedInfoParam.SetFixedInfo(fixedInfoPartyU, fixedInfoPartyV);

            var fixedInfo = _fixedInfoFactory.Get().Get(fixedInfoParam);

            var result = kdfParam.AcceptKdf(_kdfVisitor, fixedInfo);

            await Notify(new KdaAftTwoStepResult()
            {
                KdfInputs = (KdfParameterTwoStep)kdfParam,
                FixedInfoPartyU = fixedInfoPartyU,
                FixedInfoPartyV = fixedInfoPartyV,
                DerivedKeyingMaterial = result.DerivedKey
            });
        }

        private bool IncludeEphemeralData()
        {
            return _entropyProvider.GetEntropy(1).Bits[0];
        }

        private async Task KdfMultiExpand()
        {
            var kdfParam = _param.KdfConfigurationMultiExpand.GetKdfParameter(_kdfMultiExpansionParameterVisitor);
            kdfParam.Z = _entropyProvider.GetEntropy(_param.ZLength);

            var result = kdfParam.AcceptKdf(_kdfMultiExpansionVisitor);

            await Notify(new KdaAftTwoStepResult()
            {
                MultiExpansionInputs = (KdfMultiExpansionParameterTwoStep)kdfParam,
                MultiExpansionResult = result
            });
        }
    }
}
