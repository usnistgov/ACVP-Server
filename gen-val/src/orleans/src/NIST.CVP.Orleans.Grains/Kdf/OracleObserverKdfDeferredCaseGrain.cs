using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.KDF.Enums;
using NIST.CVP.Math;
using NIST.CVP.Orleans.Grains.Interfaces.Kdf;
using System.Threading.Tasks;

namespace NIST.CVP.Orleans.Grains.Kdf
{
    public class OracleObserverKdfDeferredCaseGrain : ObservableOracleGrainBase<KdfResult>,
        IOracleObserverKdfDeferredCaseGrain
    {
        private readonly IRandom800_90 _rand;

        private KdfParameters _param;

        public OracleObserverKdfDeferredCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IRandom800_90 rand
        ) : base(nonOrleansScheduler)
        {
            _rand = rand;
        }

        public async Task<bool> BeginWorkAsync(KdfParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            int keyLen = 0;
            int ivLen = 0;

            switch (_param.MacMode)
            {
                case MacModes.CMAC_AES128:
                    keyLen = 128;
                    ivLen = 128;
                    break;

                case MacModes.HMAC_SHA1:
                    keyLen = 128;
                    ivLen = 160;
                    break;

                case MacModes.HMAC_SHA224:
                case MacModes.HMAC_SHA_d512t224:
                case MacModes.HMAC_SHA3_224:
                    keyLen = 128;
                    ivLen = 224;
                    break;

                case MacModes.HMAC_SHA256:
                case MacModes.HMAC_SHA_d512t256:
                case MacModes.HMAC_SHA3_256:
                    keyLen = 128;
                    ivLen = 256;
                    break;

                case MacModes.HMAC_SHA384:
                case MacModes.HMAC_SHA3_384:
                    keyLen = 128;
                    ivLen = 384;
                    break;

                case MacModes.HMAC_SHA512:
                case MacModes.HMAC_SHA3_512:
                    keyLen = 128;
                    ivLen = 512;
                    break;

                case MacModes.CMAC_AES192:
                    keyLen = 192;
                    ivLen = 128;
                    break;

                case MacModes.CMAC_TDES:
                    keyLen = 192;
                    ivLen = 64;
                    break;

                case MacModes.CMAC_AES256:
                    keyLen = 256;
                    ivLen = 128;
                    break;
            }

            // Notify observers of result
            await Notify(new KdfResult()
            {
                KeyIn = _rand.GetRandomBitString(keyLen),
                Iv = _rand.GetRandomBitString(_param.ZeroLengthIv ? 0 : ivLen),
                FixedData = _rand.GetRandomBitString(128),
                BreakLocation = _rand.GetRandomInt(1, 128)
            });
        }
    }
}