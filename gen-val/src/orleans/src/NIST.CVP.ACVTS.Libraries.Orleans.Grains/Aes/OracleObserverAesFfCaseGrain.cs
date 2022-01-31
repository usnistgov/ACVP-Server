using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes.Ffx;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Aes;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Aes
{
    public class OracleObserverAesFfCaseGrain : ObservableOracleGrainBase<AesResult>,
        IOracleObserverAesFfCaseGrain
    {

        private readonly IRandom800_90 _random;
        private readonly IFfxModeBlockCipherFactory _aesFfxModeBlockCipherFactory;

        private AesFfParameters _param;

        public OracleObserverAesFfCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IFfxModeBlockCipherFactory aesFfxModeBlockCipherFactory,
            IRandom800_90 random
        ) : base(nonOrleansScheduler)
        {
            _aesFfxModeBlockCipherFactory = aesFfxModeBlockCipherFactory;
            _random = random;
        }

        public async Task<bool> BeginWorkAsync(AesFfParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var cipher = _aesFfxModeBlockCipherFactory.Get(_param.AlgoMode);

            List<int> payloadCandidate = new List<int>();
            for (var i = 0; i < _param.DataLength; i++)
            {
                payloadCandidate.Add(_random.GetRandomInt(0, _param.Radix));
            }

            var payload = NumeralString.ToBitString(new NumeralString(payloadCandidate.ToArray()));

            var key = _random.GetRandomBitString(_param.KeyLength);
            var iv = _random.GetRandomBitString(_param.TweakLength);

            var blockCipherParams = new FfxModeBlockCipherParameters()
            {
                Direction = _param.Direction,
                Iv = iv.GetDeepCopy(),
                Key = key.GetDeepCopy(),
                Radix = _param.Radix,
                Payload = payload.GetDeepCopy()
            };

            var result = cipher.ProcessPayload(blockCipherParams);

            if (!result.Success)
            {
                // Log error somewhere
                throw new Exception();
            }

            // Notify observers of result
            await Notify(new AesResult
            {
                PlainText = _param.Direction == BlockCipherDirections.Encrypt ? payload : result.Result,
                CipherText = _param.Direction == BlockCipherDirections.Decrypt ? payload : result.Result,
                Key = key,
                Iv = iv
            });
        }
    }
}
