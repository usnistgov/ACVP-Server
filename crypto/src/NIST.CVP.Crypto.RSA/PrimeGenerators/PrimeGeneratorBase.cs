using System.Numerics;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Crypto.Math;

namespace NIST.CVP.Crypto.RSA.PrimeGenerators
{
    public abstract class PrimeGeneratorBase : IPrimeGenerator
    {
        protected IEntropyProvider _entropyProvider;
        protected IEntropyProviderFactory _entropyProviderFactory = new EntropyProviderFactory();

        protected PrimeGen186_4 _primeGen = new PrimeGen186_4();
        private HashFunction _hashFunction;
        private PrimeTestModes _primeTestMode;
        protected int[] _bitlens = new int[4];

        #region Constructors
        protected PrimeGeneratorBase()
        {
            SetHashFunction(new HashFunction {Mode = ModeValues.SHA1, DigestSize = DigestSizes.d160});
            SetEntropyProviderType(EntropyProviderTypes.Random);
        }

        protected PrimeGeneratorBase(EntropyProviderTypes entropyProviderType)
        {
            SetEntropyProviderType(entropyProviderType);
        }

        protected PrimeGeneratorBase(HashFunction hashFunction)
        {
            SetHashFunction(hashFunction);
        }

        protected PrimeGeneratorBase(HashFunction hashFunction, EntropyProviderTypes entropyProviderType)
        {
            SetHashFunction(hashFunction);
            SetEntropyProviderType(entropyProviderType);
        }
        #endregion Constructors

        #region Sets/Adds
        public void SetHashFunction(HashFunction hashFunction)
        {
            _hashFunction = hashFunction;
        }

        public void SetPrimeTestMode(PrimeTestModes ptMode)
        {
            _primeTestMode = ptMode;
        }

        public void SetBitlens(int[] bitlens)
        {
            _bitlens = bitlens;
        }

        public void SetBitlens(int b1, int b2, int b3, int b4)
        {
            _bitlens[0] = b1;
            _bitlens[1] = b2;
            _bitlens[2] = b3;
            _bitlens[3] = b4;
        }

        public void SetEntropyProviderType(EntropyProviderTypes type)
        {
            _entropyProvider = _entropyProviderFactory.GetEntropyProvider(type);
        }

        public void AddEntropy(BigInteger entropy)
        {
            _entropyProvider.AddEntropy(entropy);
        }

        public void AddEntropy(BitString entropy)
        {
            _entropyProvider.AddEntropy(entropy);
        }
        #endregion Sets/Adds

        protected bool MillerRabin(int nlen, BigInteger val, bool factor)
        {
            if (nlen == 2048)
            {
                if (_primeTestMode == PrimeTestModes.C2)
                {
                    if (factor)
                    {
                        return NumberTheory.MillerRabin(val, 38);
                    }
                    else
                    {
                        return NumberTheory.MillerRabin(val, 5);
                    }
                }
                else // if (_primeTestMode == PrimeTestModes.C3)
                {
                    if (factor)
                    {
                        return NumberTheory.MillerRabin(val, 41);
                    }
                    else
                    {
                        return NumberTheory.MillerRabin(val, 4);
                    }
                }
            }
            else // if(nlen == 3072)
            {
                if (_primeTestMode == PrimeTestModes.C2)
                {
                    if (factor)
                    {
                        return NumberTheory.MillerRabin(val, 32);
                    }
                    else
                    {
                        return NumberTheory.MillerRabin(val, 4);
                    }
                }
                else // if (_primeTestMode == PrimeTestModes.C3)
                {
                    if (factor)
                    {
                        return NumberTheory.MillerRabin(val, 27);
                    }
                    else
                    {
                        return NumberTheory.MillerRabin(val, 3);
                    }
                }
            }
        }

        public abstract PrimeGeneratorResult GeneratePrimes(int nlen, BigInteger e, BitString seed);
    }
}
