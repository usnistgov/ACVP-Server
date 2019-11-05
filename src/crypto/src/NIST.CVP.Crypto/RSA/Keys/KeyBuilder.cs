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
        private Fips186Standard _standard;
        private int _a, _b;
        
        public KeyBuilder(IPrimeGeneratorFactory primeFactory)
        {
            _primeFactory = primeFactory;
        }

        public IKeyBuilder WithStandard(Fips186Standard standard)
        {
            _standard = standard;
            return this;
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

        public IKeyBuilder WithPMod8(int a)
        {
            _a = a;
            return this;
        }

        public IKeyBuilder WithQMod8(int b)
        {
            _b = b;
            return this;
        }

        public KeyResult Build()
        {
            if (_keyComposer == null || _e == 0 || _nlen == 0 || _standard == Fips186Standard.None)
            {
                return new KeyResult($"Invalid parameters provided. Check e, n-len, key composer, standard.");
            }

            var primeGenParams = new PrimeGeneratorParameters
            {
                Modulus = _nlen,
                BitLens = _bitlens,     // Only needed for AuxPrimes
                PublicE = _e,
                Seed = _seed,            // Only Needed for ProvablePrimes
                A = _a,
                B = _b
            };

            PrimeGeneratorResult primeResult;
            switch (_standard)
            {
                case Fips186Standard.Fips186_2:
                    primeResult = _primeFactory
                        .GetFips186_2PrimeGenerator(_entropyProvider, _primeTestMode)
                        .GeneratePrimesFips186_2(primeGenParams);
                    break;
                
                case Fips186Standard.Fips186_4:
                    primeResult = _primeFactory
                        .GetFips186_4PrimeGenerator(_primeGenMode, _sha, _entropyProvider, _primeTestMode)
                        .GeneratePrimesFips186_4(primeGenParams);
                    break;
                
                case Fips186Standard.Fips186_5:
                    primeResult = _primeFactory
                        .GetFips186_5PrimeGenerator(_primeGenMode, _sha, _entropyProvider, _primeTestMode)
                        .GeneratePrimesFips186_5(primeGenParams);
                    break;
                
                default:
                    return new KeyResult("Unable to find standard to generate key against.");
            }
            
            if (!primeResult.Success)
            {
                return new KeyResult($"Failed prime gen: {primeResult.ErrorMessage}");
            }

            var key = _keyComposer.ComposeKey(_e, primeResult.Primes);
            return new KeyResult(key, primeResult.AuxValues);
        }
    }
}
