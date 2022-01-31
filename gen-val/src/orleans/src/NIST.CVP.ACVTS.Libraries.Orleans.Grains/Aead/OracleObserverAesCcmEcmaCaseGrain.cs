using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes.Aead;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Aead;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Aead
{
    public class OracleObserverAesCcmEcmaCaseGrain : ObservableOracleGrainBase<AeadResult>,
        IOracleObserverAesCcmEcmaCaseGrain
    {
        private readonly IBlockCipherEngineFactory _engineFactory;
        private readonly IEntropyProvider _entropyProvider;
        private readonly IAeadRunner _aeadRunner;
        private readonly IAeadModeBlockCipherFactory _aeadCipherFactory;

        private AeadParameters _param;

        public OracleObserverAesCcmEcmaCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IBlockCipherEngineFactory engineFactory,
            IEntropyProviderFactory entropyProviderFactory,
            IAeadRunner aeadRunner,
            IAeadModeBlockCipherFactory aeadCipherFactory
        ) : base(nonOrleansScheduler)
        {
            _engineFactory = engineFactory;
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
            _aeadRunner = aeadRunner;
            _aeadCipherFactory = aeadCipherFactory;
        }

        public async Task<bool> BeginWorkAsync(AeadParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var flags = new BitString("59");
            var nonce = _entropyProvider.GetEntropy(13 * 8);
            var aadLen = MsbLsbConversionHelpers.ReverseByteOrder(BitString.To16BitString((short)((_param.AadLength - (14 * 8)) / 8)).ToBytes());
            var dataLen = BitString.To16BitString((short)(_param.PayloadLength / 8)).ToBytes();

            var aad = new BitString("581C")
                .ConcatenateBits(nonce.Substring(2 * 8, 2 * 8))
                .ConcatenateBits(nonce.Substring(0, 2 * 8))
                .ConcatenateBits(BitString.Zeroes(2 * 8))
                .ConcatenateBits(BitString.Zeroes(2 * 8))
                .ConcatenateBits(new BitString(aadLen))
                .ConcatenateBits(BitString.Zeroes(2 * 8))
                .ConcatenateBits(_entropyProvider.GetEntropy(_param.AadLength - (14 * 8)));

            var iv = flags
                .ConcatenateBits(nonce)
                .ConcatenateBits(new BitString(dataLen));

            var fullParams = new AeadResult
            {
                PlainText = _entropyProvider.GetEntropy(_param.PayloadLength),
                Key = _entropyProvider.GetEntropy(_param.KeyLength),
                Iv = nonce,
                Aad = aad
            };

            var returnResult = _aeadRunner.DoSimpleAead(
                _aeadCipherFactory.GetAeadCipher(
                    _engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes),
                    BlockCipherModesOfOperation.Ccm
                ),
                fullParams, _param
            );

            returnResult.Iv = iv;

            // Notify observers of result
            await Notify(returnResult);
        }
    }
}
