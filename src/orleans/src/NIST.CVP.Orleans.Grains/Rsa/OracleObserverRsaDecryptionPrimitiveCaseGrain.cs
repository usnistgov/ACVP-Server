using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Math;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Interfaces.Rsa;

namespace NIST.CVP.Orleans.Grains.Rsa
{
    public class OracleObserverRsaDecryptionPrimitiveCaseGrain : ObservableOracleGrainBase<RsaDecryptionPrimitiveResult>, 
        IOracleObserverRsaDecryptionPrimitiveCaseGrain
    {
        private readonly IKeyGenParameterHelper _keyGenHelper;
        private readonly IRsa _rsa;
        private readonly IKeyBuilder _keyBuilder;
        private readonly IKeyComposerFactory _keyComposerFactory;
        private readonly IEntropyProvider _entropyProvider;
        private readonly IRandom800_90 _rand;

        private RsaDecryptionPrimitiveParameters _param;

        public OracleObserverRsaDecryptionPrimitiveCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IKeyGenParameterHelper keyGenHelper,
            IRsa rsa,
            IKeyBuilder keyBuilder,
            IKeyComposerFactory keyComposerFactory,
            IEntropyProviderFactory entropyProviderFactory,
            IRandom800_90 rand
        ) : base (nonOrleansScheduler)
        {
            _keyGenHelper = keyGenHelper;
            _rsa = rsa;
            _keyBuilder = keyBuilder;
            _keyComposerFactory = keyComposerFactory;
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
            _rand = rand;
        }
        
        public async Task<bool> BeginWorkAsync(RsaDecryptionPrimitiveParameters param)
        {
            _param = param;
            
            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            RsaDecryptionPrimitiveResult result;

            // TODO use an additional parameter for this grain, passing in a key to be able to make use of pools.
            if (_param.TestPassed)
            {
                // Correct tests
                KeyResult keyResult;
                do
                {
                    var e = _keyGenHelper.GetEValue(RsaRunner.RSA_PUBLIC_EXPONENT_BITS_MIN, RsaRunner.RSA_PUBLIC_EXPONENT_BITS_MAX);
                    keyResult = _keyBuilder
                        .WithPrimeGenMode(PrimeGenModes.B33)
                        .WithEntropyProvider(_entropyProvider)
                        .WithNlen(_param.Modulo)
                        .WithPublicExponent(e)
                        .WithPrimeTestMode(PrimeTestModes.C2)
                        .WithKeyComposer(_keyComposerFactory.GetKeyComposer(PrivateKeyModes.Standard))
                        .Build();
                } while (!keyResult.Success);

                var cipherText = new BitString(_rand.GetRandomBigInteger(1, keyResult.Key.PubKey.N - 1));
                var plainText = _rsa.Decrypt(cipherText.ToPositiveBigInteger(), keyResult.Key.PrivKey, keyResult.Key.PubKey).PlainText;

                result = new RsaDecryptionPrimitiveResult
                {
                    CipherText = cipherText,
                    Key = keyResult.Key,
                    PlainText = new BitString(plainText, _param.Modulo, false)
                };
            }
            else
            {
                // Failure tests - save some time and generate a dummy key

                // Pick a random ciphertext and force a leading '1' (so that it MUST be 2048 bits)
                var cipherText = BitString.One().ConcatenateBits(_rand.GetRandomBitString(_param.Modulo - 1));

                // Pick a random n that is 2048 bits and less than the ciphertext
                var n = _rand.GetRandomBigInteger(NumberTheory.Pow2(_param.Modulo - 1), cipherText.ToPositiveBigInteger());
                var e = _keyGenHelper.GetEValue(RsaRunner.RSA_PUBLIC_EXPONENT_BITS_MIN, RsaRunner.RSA_PUBLIC_EXPONENT_BITS_MAX).ToPositiveBigInteger();

                result = new RsaDecryptionPrimitiveResult
                {
                    CipherText = cipherText,
                    Key = new KeyPair { PubKey = new PublicKey { E = e, N = n } }
                };
            }

            // Notify observers of result
            await Notify(result);
        }
    }
}