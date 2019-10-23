using System.Numerics;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators
{
    public class PrimeGeneratorParameters
    {
        // Required
        public int Modulus { get; set; }
        public BitString Seed { get; set; }
        public BigInteger PublicE { get; set; }
        
        // Sometimes Required
        public int[] BitLens { get; set; } = new int[4];       // Req for any with probable auxiliary primes
        
        // Optional
        public int A { get; set; }                // Opt for any with probable primes
        public int B { get; set; }                // Opt for any with probable primes
    }
}