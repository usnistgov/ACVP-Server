using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.IKEv1;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.IKEv1.Enums;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Kdf;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Kdf
{
    public class OracleObserverIkeV1KdfCaseGrain : ObservableOracleGrainBase<IkeV1KdfResult>,
        IOracleObserverIkeV1KdfCaseGrain
    {
        private readonly IIkeV1Factory _kdfFactory;
        private readonly IRandom800_90 _rand;

        private IkeV1KdfParameters _param;

        public OracleObserverIkeV1KdfCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IIkeV1Factory kdfFactory,
            IRandom800_90 rand
        ) : base(nonOrleansScheduler)
        {
            _kdfFactory = kdfFactory;
            _rand = rand;
        }

        public async Task<bool> BeginWorkAsync(IkeV1KdfParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var ike = _kdfFactory.GetIkeV1Instance(_param.AuthenticationMethod, _param.HashAlg);

            var nInit = _rand.GetRandomBitString(_param.NInitLength);
            var nResp = _rand.GetRandomBitString(_param.NRespLength);
            var ckyInit = _rand.GetRandomBitString(64);
            var ckyResp = _rand.GetRandomBitString(64);
            var gxy = _rand.GetRandomBitString(_param.GxyLength);
            var preSharedKey = _param.AuthenticationMethod == AuthenticationMethods.Psk ? _rand.GetRandomBitString(_param.PreSharedKeyLength) : null;

            var result = ike.GenerateIke(nInit, nResp, gxy, ckyInit, ckyResp, preSharedKey);
            if (!result.Success)
            {
                throw new Exception();
            }

            // Notify observers of result
            await Notify(new IkeV1KdfResult
            {
                CkyInit = ckyInit,
                CkyResp = ckyResp,
                Gxy = gxy,
                NInit = nInit,
                NResp = nResp,
                PreSharedKey = preSharedKey,
                sKeyId = result.SKeyId,
                sKeyIdA = result.SKeyIdA,
                sKeyIdD = result.SKeyIdD,
                sKeyIdE = result.SKeyIdE
            });
        }
    }
}
