using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.SRTP;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Kdf;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Kdf
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
        ) : base(nonOrleansScheduler)
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
            var srtcpIndex = BitString.Zero().ConcatenateBits(_rand.GetRandomBitString(31));

            if (_param.Supports48BitSrtcpIndex == true)
            {
                srtcpIndex = BitString.Zeroes(16).ConcatenateBits(srtcpIndex);
            }
            
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
