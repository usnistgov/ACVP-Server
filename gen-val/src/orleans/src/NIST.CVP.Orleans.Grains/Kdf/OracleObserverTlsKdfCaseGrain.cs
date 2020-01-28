using System;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.KDF.Components.TLS;
using NIST.CVP.Math;
using NIST.CVP.Orleans.Grains.Interfaces.Kdf;
using TlsKdfResult = NIST.CVP.Common.Oracle.ResultTypes.TlsKdfResult;

namespace NIST.CVP.Orleans.Grains.Kdf
{
    public class OracleObserverTlsKdfCaseGrain : ObservableOracleGrainBase<TlsKdfResult>, 
        IOracleObserverTlsKdfCaseGrain
    {
        private readonly ITlsKdfFactory _kdfFactory;
        private readonly IRandom800_90 _rand;
        
        private TlsKdfParameters _param;

        public OracleObserverTlsKdfCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            ITlsKdfFactory kdfFactory,
            IRandom800_90 rand
        ) : base (nonOrleansScheduler)
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

            var preMasterSecret = _rand.GetRandomBitString(_param.PreMasterSecretLength);
            var clientHelloRandom = _rand.GetRandomBitString(256);
            var serverHelloRandom = _rand.GetRandomBitString(256);
            var clientRandom = _rand.GetRandomBitString(256);
            var serverRandom = _rand.GetRandomBitString(256);

            var result = tls.DeriveKey(preMasterSecret, clientHelloRandom, serverHelloRandom, clientRandom, serverRandom, _param.KeyBlockLength);
            if (!result.Success)
            {
                throw new Exception();
            }

            // Notify observers of result
            await Notify(new TlsKdfResult
            {
                PreMasterSecret = preMasterSecret,
                ClientHelloRandom = clientHelloRandom,
                ClientRandom = clientRandom,
                KeyBlock = result.DerivedKey,
                MasterSecret = result.MasterSecret,
                ServerHelloRandom = serverHelloRandom,
                ServerRandom = serverRandom
            });
        }
    }
}