using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.TLS;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Kdf;
using TlsKdfResult = NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.TlsKdfResult;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Kdf
{
    public class OracleObserverTlsEmsKdfCaseGrain : ObservableOracleGrainBase<TlsKdfResult>,
        IOracleObserverTlsEmsKdfCaseGrain
    {
        private readonly ITlsKdfFactory _kdfFactory;
        private readonly IRandom800_90 _rand;

        private TlsKdfParameters _param;

        public OracleObserverTlsEmsKdfCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            ITlsKdfFactory kdfFactory,
            IRandom800_90 rand
        ) : base(nonOrleansScheduler)
        {
            _kdfFactory = kdfFactory;
            _rand = rand;
        }

        public async Task<bool> BeginWorkAsync(TlsKdfParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var tls = _kdfFactory.GetTlsKdfInstance(_param.TlsMode, _param.HashAlg);

            var shaAttributes = ShaAttributes.GetShaAttributes(_param.HashAlg.Mode, _param.HashAlg.DigestSize);

            var preMasterSecret = _rand.GetRandomBitString(_param.PreMasterSecretLength);
            var sessionHash = _rand.GetRandomBitString(shaAttributes.outputLen);
            var clientRandom = _rand.GetRandomBitString(256);
            var serverRandom = _rand.GetRandomBitString(256);

            var result = tls.DeriveKey(
                preMasterSecret,
                sessionHash,
                new BitString(0),
                clientRandom,
                serverRandom,
                _param.KeyBlockLength);
            if (!result.Success)
            {
                throw new Exception();
            }

            // Notify observers of result
            await Notify(new TlsKdfResult
            {
                SessionHash = sessionHash,
                PreMasterSecret = preMasterSecret,
                ClientRandom = clientRandom,
                ServerRandom = serverRandom,
                KeyBlock = result.DerivedKey,
                MasterSecret = result.MasterSecret,
            });
        }
    }
}
