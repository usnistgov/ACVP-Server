using System;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.KDF.Components.SSH;
using NIST.CVP.Math;
using NIST.CVP.Orleans.Grains.Interfaces.Kdf;

namespace NIST.CVP.Orleans.Grains.Kdf
{
    public class OracleObserverSshKdfCaseGrain : ObservableOracleGrainBase<SshKdfResult>, 
        IOracleObserverSshKdfCaseGrain
    {
        private readonly ISshFactory _kdfFactory;
        private readonly IRandom800_90 _rand;
        
        private SshKdfParameters _param;

        public OracleObserverSshKdfCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            ISshFactory kdfFactory,
            IRandom800_90 rand
        ) : base (nonOrleansScheduler)
        {
            _kdfFactory = kdfFactory;
            _rand = rand;
        }
        
        public async Task<bool> BeginWorkAsync(SshKdfParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            var ssh = _kdfFactory.GetSshInstance(_param.HashAlg, _param.Cipher);

            var k = _rand.GetRandomBitString(2048);

            // If the MSbit is a 1, append "00" to the front
            if (k.GetMostSignificantBits(1).Equals(BitString.One()))
            {
                k = BitString.Zeroes(8).ConcatenateBits(k);
            }

            // Append the length (32-bit) to the front (in bytes, so 256 or 257 bytes)
            var fullK = BitString.To32BitString(k.BitLength / 8).ConcatenateBits(k);

            var h = _rand.GetRandomBitString(_param.HashAlg.OutputLen);
            var sessionId = _rand.GetRandomBitString(_param.HashAlg.OutputLen);

            var result = ssh.DeriveKey(fullK, h, sessionId);
            if (!result.Success)
            {
                throw new Exception();
            }

            // Notify observers of result
            await Notify(new SshKdfResult
            {
                H = h,
                K = fullK,
                SessionId = sessionId,
                EncryptionKeyClient = result.ClientToServer.EncryptionKey,
                EncryptionKeyServer = result.ServerToClient.EncryptionKey,
                InitialIvClient = result.ClientToServer.InitialIv,
                InitialIvServer = result.ServerToClient.InitialIv,
                IntegrityKeyClient = result.ClientToServer.IntegrityKey,
                IntegrityKeyServer = result.ServerToClient.IntegrityKey
            });
        }
    }
}