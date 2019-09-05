using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using System.Numerics;

namespace NIST.CVP.Crypto.RSA.Keys
{
    public class KeyBuilder : IKeyBuilder
    {
        private ISha _sha;
        private PrimeTestModes _primeTestMode;
        private int[] _bitlens;
        private BigInteger _e;
        private int _nlen;
        private BitString _seed;
        private IEntropyProvider _entropyProvider;
        private PrimeGenModes _primeGenMode;
        private IRsaKeyComposer _keyComposer;
        private readonly IPrimeGeneratorFactory _primeFactory;

        public KeyBuilder(IPrimeGeneratorFactory primeFactory)
        {
            _primeFactory = primeFactory;
        }

        public IKeyBuilder WithHashFunction(ISha sha)
        {
            _sha = sha;
            return this;
        }

        public IKeyBuilder WithPrimeTestMode(PrimeTestModes primeTestMode)
        {
            _primeTestMode = primeTestMode;
            return this;
        }

        public IKeyBuilder WithBitlens(int[] bitlens)
        {
            _bitlens = bitlens;
            return this;
        }

        public IKeyBuilder WithPublicExponent(BigInteger e)
        {
            _e = e;
            return this;
        }

        public IKeyBuilder WithPublicExponent(BitString e)
        {
            _e = e.ToPositiveBigInteger();
            return this;
        }

        public IKeyBuilder WithSeed(BitString seed)
        {
            _seed = seed;
            return this;
        }

        public IKeyBuilder WithNlen(int nlen)
        {
            _nlen = nlen;
            return this;
        }

        public IKeyBuilder WithEntropyProvider(IEntropyProvider entropyProvider)
        {
            _entropyProvider = entropyProvider;
            return this;
        }

        public IKeyBuilder WithPrimeGenMode(PrimeGenModes primeGenMode)
        {
            _primeGenMode = primeGenMode;
            return this;
        }

        public IKeyBuilder WithKeyComposer(IRsaKeyComposer keyComposer)
        {
            _keyComposer = keyComposer;
            return this;
        }

        public KeyResult Build()
        {
            if (_keyComposer == null || _e == 0 || _nlen == 0)
            {
                return new KeyResult($"Invalid parameters provided. Check e, n-len, key composer.");
            }

            var primeGen = _primeFactory.GetPrimeGenerator(_primeGenMode, _sha, _entropyProvider, _primeTestMode);
            primeGen.SetBitlens(_bitlens);

            var primeResult = primeGen.GeneratePrimes(_nlen, _e, _seed);

            if (!primeResult.Success)
            {
                return new KeyResult($"Failed prime gen: {primeResult.ErrorMessage}");
            }

            var key = _keyComposer.ComposeKey(_e, primeResult.Primes);
            return new KeyResult(key, primeResult.AuxValues);
        }
    }
}
