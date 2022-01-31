using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes.XTS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Helpers;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Aes;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Aes
{
    public class OracleObserverAesXtsCaseGrain : ObservableOracleGrainBase<AesXtsResult>,
        IOracleObserverAesXtsCaseGrain
    {
        private readonly IXtsModeBlockCipherFactory _modeFactory;
        private readonly IRandom800_90 _rand;

        private AesXtsParameters _param;

        public OracleObserverAesXtsCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IXtsModeBlockCipherFactory modeFactory,
            IRandom800_90 rand
        ) : base(nonOrleansScheduler)
        {
            _modeFactory = modeFactory;
            _rand = rand;
        }

        public async Task<bool> BeginWorkAsync(AesXtsParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var cipher = _modeFactory.GetXtsCipher();

            var direction = _param.Direction.ToLower() == "encrypt" ? BlockCipherDirections.Encrypt : BlockCipherDirections.Decrypt;
            var payload = _rand.GetRandomBitString(_param.DataLength);
            var key = _rand.GetRandomBitString(_param.KeyLength * 2);
            BitString i;
            var number = 0;

            switch (_param.TweakMode.ToLower())
            {
                case "hex":
                    i = _rand.GetRandomBitString(128);    // Technically there are no checks in XTS if this value is too close to the upper boundary, but the chance of running into such a value is like 2^-112 worst case                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            
                    break;
                case "number":
                    number = _rand.GetRandomInt(0, 256);
                    i = XtsHelper.GetIFromInteger(number);
                    break;
                default:
                    throw new ArgumentException($"Invalid {nameof(_param.TweakMode)} provided to XTS");
            }

            var blockCipherParams = new XtsModeBlockCipherParameters(direction, i, key, payload, payload.BitLength);
            var result = cipher.ProcessPayload(blockCipherParams);

            // Notify observers of result
            await Notify(new AesXtsResult
            {
                PlainText = direction == BlockCipherDirections.Encrypt ? payload : result.Result,
                CipherText = direction == BlockCipherDirections.Decrypt ? payload : result.Result,
                SequenceNumber = number,
                Iv = i,
                Key = key
            });
        }
    }
}
