using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes.XTS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Aes;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Aes
{
    public class OracleObserverAesCompleteDeferredXtsCaseGrain : ObservableOracleGrainBase<AesXtsResult>,
        IOracleObserverAesCompleteDeferredXtsCaseGrain
    {
        private readonly IXtsModeBlockCipherFactory _modeFactory;

        private AesXtsParameters _param;
        private AesXtsResult _fullParam;

        public OracleObserverAesCompleteDeferredXtsCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IXtsModeBlockCipherFactory modeFactory
        ) : base(nonOrleansScheduler)
        {
            _modeFactory = modeFactory;
        }

        public async Task<bool> BeginWorkAsync(AesXtsParameters param, AesXtsResult fullParam)
        {
            _param = param;
            _fullParam = fullParam;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var cipher = _modeFactory.GetXtsCipher();

            var direction = _param.Direction.ToLower() == "encrypt" ? BlockCipherDirections.Encrypt : BlockCipherDirections.Decrypt;
            var payload = direction == BlockCipherDirections.Encrypt ? _fullParam.PlainText : _fullParam.CipherText;

            var blockCipherParams = new XtsModeBlockCipherParameters(direction, _fullParam.Iv, _fullParam.Key, payload, _param.DataUnitLength);
            var result = cipher.ProcessPayload(blockCipherParams);

            if (!result.Success)
            {
                // Log error somewhere
                throw new Exception();
            }

            // Notify observers of result
            await Notify(new AesXtsResult
            {
                PlainText = direction == BlockCipherDirections.Encrypt ? payload : result.Result,
                CipherText = direction == BlockCipherDirections.Decrypt ? payload : result.Result,
                SequenceNumber = _fullParam.SequenceNumber,
                Iv = _fullParam.Iv,
                Key = _fullParam.Key
            });
        }
    }
}
