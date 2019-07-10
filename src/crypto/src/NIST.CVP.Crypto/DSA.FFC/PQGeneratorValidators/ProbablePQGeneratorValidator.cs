using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Math;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Math.Helpers;
using System.Numerics;

namespace NIST.CVP.Crypto.DSA.FFC.PQGeneratorValidators
{
    public class ProbablePQGeneratorValidator : IPQGeneratorValidator
    {
        private readonly ISha _sha;
        private IEntropyProvider _entropy;
        private IEntropyProviderFactory _entropyFactory = new EntropyProviderFactory();
        private readonly IRandom800_90 _rand = new Random800_90();

        public ProbablePQGeneratorValidator(ISha sha, EntropyProviderTypes entropyType = EntropyProviderTypes.Random)
        {
            _sha = sha;
            _entropy = _entropyFactory.GetEntropyProvider(entropyType);
        }

        public void AddEntropy(BitString entropy)
        {
            _entropy.AddEntropy(entropy);
        }

        /// <summary>
        /// A.1.1.2 from FIPS 186-4
        /// </summary>
        /// <param name="L"></param>
        /// <param name="N"></param>
        /// <param name="seedLen"></param>
        /// <returns></returns>
        public PQGenerateResult Generate(int L, int N, int seedLen)
        {
            // 1. Check L/N pair
            if (!DSAHelper.VerifyLenPair(L, N))
            {
                return new PQGenerateResult("Invalid L, N pair");
            }

            // 2. Check seedLen
            if (seedLen < N)
            {
                return new PQGenerateResult("Invalid seedLen");
            }

            // 3, 4 Compute n, b
            var outLen = _sha.HashFunction.OutputLen;
            var n = L.CeilingDivide(outLen) - 1;
            var b = L - 1 - (n * outLen);

            do
            {
                BigInteger seed, q;

                do
                {
                    // 5. Get random seed
                    seed = _entropy.GetEntropy(seedLen).ToPositiveBigInteger();

                    // 6. Hash seed
                    var U = _sha.HashNumber(seed).ToBigInteger() % NumberTheory.Pow2(N - 1);

                    // 7. Compute q
                    q = NumberTheory.Pow2(N - 1) + U + 1 - (U % 2);

                    // Check if q is prime, if not go back to 5, assume highest security strength
                } while (!NumberTheory.MillerRabin(q, DSAHelper.GetMillerRabinIterations(L, N)));

                // 10, 11 Compute p
                var offset = 1;
                var upperBound = (4 * L - 1);
                for (var ctr = 0; ctr <= upperBound; ctr++)
                {
                    // 11.1, 11.2
                    var W = _sha.HashNumber(seed + offset).ToBigInteger();
                    for (var j = 1; j < n; j++)
                    {
                        W += _sha.HashNumber(seed + offset + j).ToBigInteger() * NumberTheory.Pow2(j * outLen);
                    }
                    W += (_sha.HashNumber(seed + offset + n).ToBigInteger() % NumberTheory.Pow2(b)) * NumberTheory.Pow2(n * outLen);

                    // 11.3
                    var X = W + NumberTheory.Pow2(L - 1);

                    // 11.4
                    var c = X % (2 * q);

                    // 11.5
                    var p = X - (c - 1);

                    // 11.6, 11.7, 11.8
                    if (p >= NumberTheory.Pow2(L - 1))
                    {
                        // Check if p is prime, if so return
                        if (NumberTheory.MillerRabin(p, DSAHelper.GetMillerRabinIterations(L, N)))
                        {
                            return new PQGenerateResult(p, q, new DomainSeed(seed), new Counter(ctr));
                        }
                    }

                    // 11.9
                    offset += n + 1;
                }

                // 12
            } while (true);
        }

        /// <summary>
        /// A.1.1.3 from FIPS 186-4
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <param name="seed"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public PQValidateResult Validate(BigInteger p, BigInteger q, DomainSeed seed, Counter count)
        {
            if (seed.Mode != PrimeGenMode.Probable && count.Mode != PrimeGenMode.Probable)
            {
                return new PQValidateResult("Invalid DomainSeed and Counter");
            }

            // 1, 2
            var L = new BitString(p).BitLength;
            var N = new BitString(q).BitLength;

            // 3
            if (!DSAHelper.VerifyLenPair(L, N))
            {
                return new PQValidateResult("Invalid L, N pair");
            }

            // 4
            if (count.Count > (4 * L - 1))
            {
                return new PQValidateResult("Invalid counter");
            }

            // 5, 6
            // TODO is there a better way to do this?
            /*
                Appending 0s to the bitstring representation of the seed as to make it mod 32 (if it isn't already), as this is the mod of the original seed that is hashed.
                In instances (as an example) when the chosen seed starts with eight zero bits in a row, the biginteger representation of said bitstring is 1 byte smaller than it should be,
                thus failing the check that it is at least the length of N
            */
            var seedBitString = new BitString(seed.Seed);
            if (seedBitString.BitLength % 32 != 0)
            {
                seedBitString = BitString.ConcatenateBits(BitString.Zeroes(32 - seedBitString.BitLength % 32), seedBitString);
            }
            var seedLen = seedBitString.BitLength;
            if (seedLen < N)
            {
                return new PQValidateResult("Invalid seed");
            }

            // 7
            var U = _sha.HashNumber(seed.Seed).ToBigInteger() % NumberTheory.Pow2(N - 1);

            // 8
            var computed_q = NumberTheory.Pow2(N - 1) + U + 1 - (U % 2);

            // 9
            if (!NumberTheory.MillerRabin(computed_q, DSAHelper.GetMillerRabinIterations(L, N)) || computed_q != q)
            {
                return new PQValidateResult("Q not prime, or doesn't match expected value");
            }

            // 10, 11, 12
            var outLen = _sha.HashFunction.OutputLen;
            var n = L.CeilingDivide(outLen) - 1;
            var b = L - 1 - (n * outLen);
            var offset = 1;

            // 13
            BigInteger computed_p;
            int i;
            for (i = 0; i <= count.Count; i++)
            {
                // 13.1, 13.2
                var W = _sha.HashNumber(seed.Seed + offset).ToBigInteger();
                for (var j = 1; j < n; j++)
                {
                    W += (_sha.HashNumber(seed.Seed + offset + j).ToBigInteger()) * NumberTheory.Pow2(j * outLen);
                }
                W += ((_sha.HashNumber(seed.Seed + offset + n).ToBigInteger()) % NumberTheory.Pow2(b)) * NumberTheory.Pow2(n * outLen);

                // 13.3
                var X = W + NumberTheory.Pow2(L - 1);

                // 13.4
                var c = X % (2 * q);

                // 13.5
                computed_p = X - (c - 1);

                // 13.6, 13.7, 13.8
                if (computed_p >= NumberTheory.Pow2(L - 1))
                {
                    // Check if p is prime, if so return
                    if (NumberTheory.MillerRabin(computed_p, DSAHelper.GetMillerRabinIterations(L, N)))
                    {
                        break;
                    }
                }

                // 13.9
                offset += n + 1;
            }

            // 14
            if (i != count.Count || computed_p != p || !NumberTheory.MillerRabin(computed_p, DSAHelper.GetMillerRabinIterations(L, N)))
            {
                return new PQValidateResult($"Invalid p value or counter. computed_p = {new BitString(computed_p).ToHex()}");
            }

            // 15
            return new PQValidateResult();
        }
    }
}
