using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.CTR.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Aes;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Aes
{
    public class ObserverAesCounterRfcCaseGrain : ObservableOracleGrainBase<AesResult>,
        IObserverAesCounterRfcCaseGrain
    {
        private readonly IBlockCipherEngineFactory _engineFactory;
        private readonly IModeBlockCipherFactory _modeFactory;
        private readonly ICounterFactory _counterFactory;
        private readonly IEntropyProvider _entropyProvider;

        private CounterParameters<AesParameters> _param;

        public ObserverAesCounterRfcCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IBlockCipherEngineFactory engineFactory,
            IModeBlockCipherFactory modeFactory,
            ICounterFactory counterFactory,
            IEntropyProviderFactory entropyProviderFactory
        ) : base(nonOrleansScheduler)
        {
            _engineFactory = engineFactory;
            _modeFactory = modeFactory;
            _counterFactory = counterFactory;
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
        }

        public async Task<bool> BeginWorkAsync(CounterParameters<AesParameters> param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            try
            {
                // This is a non deferred case, and is used for both encrypt and decrypt.
                // Make up the payload, then encrypt it.
                var direction = BlockCipherDirections.Encrypt;
                var payload = _entropyProvider.GetEntropy(_param.Parameters.DataLength);
                var key = _entropyProvider.GetEntropy(_param.Parameters.KeyLength);
                // For the RFC tests we want to make sure the IV's LSB is 32 bits representing the integer 1.
                var iv = _entropyProvider.GetEntropy(128 - 32)
                    .ConcatenateBits(BitString.To32BitString(1));

                var engine = _engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes);
                var counter = _counterFactory.GetCounter(
                    engine,
                    CounterTypes.Additive,
                    iv
                );
                var cipher = _modeFactory.GetCounterCipher(
                    engine,
                    counter
                );

                var blockCipherParams = new CounterModeBlockCipherParameters(
                    direction, iv, key, payload, null, false);

                var result = cipher.ProcessPayload(blockCipherParams);

                // Notify observers of result
                await Notify(new AesResult
                {
                    Key = key,
                    Iv = iv,
                    PlainText = payload,
                    CipherText = result.Result
                });
            }
            catch (Exception e)
            {
                await Throw(e);
            }
        }
    }
}
