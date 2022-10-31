using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.IKEv2;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Kdf;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Kdf
{
    public class OracleObserverIkeV2KdfCaseGrain : ObservableOracleGrainBase<IkeV2KdfResult>,
        IOracleObserverIkeV2KdfCaseGrain
    {
        private readonly IIkeV2Factory _kdfFactory;
        private readonly IRandom800_90 _rand;

        private IkeV2KdfParameters _param;

        public OracleObserverIkeV2KdfCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IIkeV2Factory kdfFactory,
            IRandom800_90 rand
        ) : base(nonOrleansScheduler)
        {
            _kdfFactory = kdfFactory;
            _rand = rand;
        }

        public async Task<bool> BeginWorkAsync(IkeV2KdfParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var ike = _kdfFactory.GetInstance(_param.HashAlg);

            var nInit = _rand.GetRandomBitString(_param.NInitLength);
            var nResp = _rand.GetRandomBitString(_param.NRespLength);
            var gir = _rand.GetRandomBitString(_param.GirLength);
            var girNew = _rand.GetRandomBitString(_param.GirLength);
            var spiInit = _rand.GetRandomBitString(64);
            var spiResp = _rand.GetRandomBitString(64);

            var result = ike.GenerateIke(nInit, nResp, gir, girNew, spiInit, spiResp, _param.DerivedKeyingMaterialLength, _param.DerivedKeyingMaterialChildLength);
            if (!result.Success)
            {
                throw new Exception();
            }

            // Notify observers of result
            await Notify(new IkeV2KdfResult
            {
                DerivedKeyingMaterial = result.DKM,
                NResp = nResp,
                NInit = nInit,
                DerivedKeyingMaterialChild = result.DKMChildSA,
                DerivedKeyingMaterialDh = result.DKMChildSADh,
                Gir = gir,
                GirNew = girNew,
                SKeySeed = result.SKeySeed,
                SKeySeedReKey = result.SKeySeedReKey,
                SpiInit = spiInit,
                SpiResp = spiResp
            });
        }
    }
}
