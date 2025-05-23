﻿using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfTwoStep;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Cr1;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Cr1;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Kas.Sp800_56Cr1;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Kas.Sp800_56Cr1
{
    public class ObserverKdaValTwoStepCaseGrain : ObservableOracleGrainBase<KdaValTwoStepResult>, IObserverKdaValTwoStepCaseGrain
    {
        private const int LengthPartyId = 128;

        private readonly IKdfParameterVisitor _kdfParameterVisitor;
        private readonly IKdfVisitor _kdfVisitor;
        private readonly IEntropyProvider _entropyProvider;
        private readonly IRandom800_90 _random;
        private readonly IFixedInfoFactory _fixedInfoFactory;
        private readonly IKdfMultiExpansionParameterVisitor _kdfMultiExpansionParameterVisitor;
        private readonly IKdfMultiExpansionVisitor _kdfMultiExpansionVisitor;

        private KdaValTwoStepParameters _param;


        public ObserverKdaValTwoStepCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IKdfParameterVisitor kdfParameterVisitor,
            IKdfVisitor kdfVisitor,
            IEntropyProvider entropyProvider,
            IRandom800_90 random,
            IFixedInfoFactory fixedInfoFactory,
            IKdfMultiExpansionParameterVisitor kdfMultiExpansionParameterVisitor,
            IKdfMultiExpansionVisitor kdfMultiExpansionVisitor)
            : base(nonOrleansScheduler)
        {
            _kdfParameterVisitor = kdfParameterVisitor;
            _kdfVisitor = kdfVisitor;
            _entropyProvider = entropyProvider;
            _random = random;
            _fixedInfoFactory = fixedInfoFactory;
            _kdfMultiExpansionParameterVisitor = kdfMultiExpansionParameterVisitor;
            _kdfMultiExpansionVisitor = kdfMultiExpansionVisitor;
        }

        public async Task<bool> BeginWorkAsync(KdaValTwoStepParameters param)
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
            var testPassed = true;

            while (true)
            {
                var kdfParam = _kdfParameterVisitor.CreateParameter(_param.KdfConfiguration);
                kdfParam.Z = _entropyProvider.GetEntropy(_param.ZLength);
                // Does IUT support/use a hybrid shared secret?
                if (_param.AuxSharedSecretLen != default)
                {
                    kdfParam.T = _entropyProvider.GetEntropy(_param.AuxSharedSecretLen);
                }
                
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
                    EntropyBits = kdfParam.EntropyBits,
                };
                fixedInfoParam.SetFixedInfo(fixedInfoPartyU, fixedInfoPartyV);

                var fixedInfo = _fixedInfoFactory.Get().Get(fixedInfoParam);

                var result = kdfParam.AcceptKdf(_kdfVisitor, fixedInfo);

                switch (_param.Disposition)
                {
                    case KdaTestCaseDisposition.SuccessLeadingZeroNibble when result.DerivedKey[0] >= 0x10:
                        continue;
                    case KdaTestCaseDisposition.Fail:
                        kdfParam.Z = _random.GetDifferentBitStringOfSameSize(kdfParam.Z);
                        // Does IUT support/use a hybrid shared secret?
                        if (_param.AuxSharedSecretLen != default)
                        {
                            kdfParam.T = _random.GetDifferentBitStringOfSameSize(kdfParam.T); 
                        }
                        testPassed = false;
                        break;
                }

                await Notify(new KdaValTwoStepResult()
                {
                    KdfInputs = (KdfParameterTwoStep)kdfParam,
                    FixedInfoPartyU = fixedInfoPartyU,
                    FixedInfoPartyV = fixedInfoPartyV,
                    DerivedKeyingMaterial = result.DerivedKey,
                    TestPassed = testPassed
                });
                break;
            }
        }

        private bool IncludeEphemeralData()
        {
            return _entropyProvider.GetEntropy(1).Bits[0];
        }

        private async Task KdfMultiExpand()
        {
            var testPassed = true;

            while (true)
            {
                var kdfParam = _kdfMultiExpansionParameterVisitor.CreateParameter(_param.KdfConfigurationMultiExpand);
                kdfParam.Z = _entropyProvider.GetEntropy(_param.ZLength);
                // Does the IUT support/use a hybrid shared secret?
                if (_param.AuxSharedSecretLen != default)
                {
                    kdfParam.T = _entropyProvider.GetEntropy(_param.AuxSharedSecretLen);
                }
                
                var result = kdfParam.AcceptKdf(_kdfMultiExpansionVisitor);

                switch (_param.Disposition)
                {
                    case KdaTestCaseDisposition.SuccessLeadingZeroNibble when result.Results[0].DerivedKey[0] >= 0x10:
                        continue;
                    case KdaTestCaseDisposition.Fail:
                        kdfParam.Z = _random.GetDifferentBitStringOfSameSize(kdfParam.Z);
                        // Does IUT support/use a hybrid shared secret?
                        if (_param.AuxSharedSecretLen != default)
                        {
                            kdfParam.T = _random.GetDifferentBitStringOfSameSize(kdfParam.T); 
                        }
                        testPassed = false;
                        break;
                }

                await Notify(new KdaValTwoStepResult()
                {
                    MultiExpansionInputs = (KdfMultiExpansionParameterTwoStep)kdfParam,
                    MultiExpansionResult = result,
                    TestPassed = testPassed
                });
                break;
            }
        }
    }
}
