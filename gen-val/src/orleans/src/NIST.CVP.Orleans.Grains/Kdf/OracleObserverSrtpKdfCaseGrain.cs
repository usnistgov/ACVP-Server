using System;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.KDF.Components.SRTP;
using NIST.CVP.Math;
using NIST.CVP.Orleans.Grains.Interfaces.Kdf;

namespace NIST.CVP.Orleans.Grains.Kdf
{
    public class OracleObserverSrtpKdfCaseGrain : ObservableOracleGrainBase<SrtpKdfResult>, 
        IOracleObserverSrtpKdfCaseGrain
    {
        private readonly ISrtpFactory _kdfFactory;
        private readonly IRandom800_90 _rand;
        
        private SrtpKdfParameters _param;

        public OracleObserverSrtpKdfCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            ISrtpFactory kdfFactory,
            IRandom800_90 rand
        ) : base (nonOrleansScheduler)
        {
            _kdfFactory = kdfFactory;
            _rand = rand;
        }
        
        public async Task<bool> BeginWorkAsync(SrtpKdfParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            var key = _rand.GetRandomBitString(_param.AesKeyLength);
            var salt = _rand.GetRandomBitString(112);
            var index = _rand.GetRandomBitString(48);
            var srtcpIndex = _rand.GetRandomBitString(32);

            var result = _kdfFactory.GetInstance()
                .DeriveKey(_param.AesKeyLength, key, salt, _param.KeyDerivationRate, index, srtcpIndex);
            if (!result.Success)
            {
                throw new Exception();
            }

            // Notify observers of result
            await Notify(new SrtpKdfResult
            {
                Index = index,
                MasterKey = key,
                MasterSalt = salt,
                SrtcpIndex = srtcpIndex,
                SrtcpAuthenticationKey = result.SrtcpResult.AuthenticationKey,
                SrtcpEncryptionKey = result.SrtcpResult.EncryptionKey,
                SrtcpSaltingKey = result.SrtcpResult.SaltingKey,
                SrtpAuthenticationKey = result.SrtpResult.AuthenticationKey,
                SrtpEncryptionKey = result.SrtpResult.EncryptionKey,
                SrtpSaltingKey = result.SrtpResult.SaltingKey
            });
        }
    }
}