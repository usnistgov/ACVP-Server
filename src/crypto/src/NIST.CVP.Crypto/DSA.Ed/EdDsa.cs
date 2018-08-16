using System;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Math;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.DSA.Ed
{
    public class EdDsa : IDsaEd
    {
        public ISha Sha { get; set; }

        private readonly IEntropyProviderFactory _entropyFactory = new EntropyProviderFactory();
        private readonly IEntropyProvider _entropyProvider;

        public EdDsa(EntropyProviderTypes entropyType = EntropyProviderTypes.Random)
        {
            _entropyProvider = _entropyFactory.GetEntropyProvider(entropyType);
        }

        public void AddEntropy(BigInteger entropy)
        {
            _entropyProvider.AddEntropy(entropy);
        }

        public EdDomainParametersGenerateResult GenerateDomainParameters(EdDomainParametersGenerateRequest generateRequest)
        {
            throw new NotImplementedException();
        }

        public EdDomainParametersValidateResult ValidateDomainParameters(EdDomainParametersValidateRequest domainParameters)
        {
            throw new NotImplementedException();
        }

        public EdKeyPairGenerateResult GenerateKeyPair(EdDomainParameters domainParameters)
        {
            // Generate random number k ... not sure what the range should be for this.
            var k = _entropyProvider.GetEntropy(1, NumberTheory.Pow2(domainParameters.CurveE.VariableB + 1) - 1);

            // 1. Hash the private key
            Sha = domainParameters.Hash;
            // 912 is the output length for Ed448 when using SHAKE. It will not affect SHA512 output length for Ed25519.
            var h = Sha.HashMessage(new BitString(k, domainParameters.CurveE.VariableB), 912).Digest.MSBSubstring(0, domainParameters.CurveE.VariableB);
            h = BitString.ReverseByteOrder(h);

            // 2. Prune the buffer
            // Currently this is accomplished using the specifications in IETF RFC 8032
            // Could switch over to how it is specified in SP186-5 in the future
            for (int i = 0; i < domainParameters.CurveE.VariableC; i++)
            {
                h.Bits.Set(i, false);
            }
            for (int i = domainParameters.CurveE.VariableN; i < h.Bits.Length; i++)
            {
                h.Bits.Set(i, false);
            }

            // 3. Determine s
            var s = NumberTheory.Pow2(domainParameters.CurveE.VariableN) + h.ToPositiveBigInteger();

            // 4. Compute Q such that Q = s * G
            var Q = domainParameters.CurveE.Multiply(domainParameters.CurveE.BasePointG, s);

            // Return key pair (Q, d)
            return new EdKeyPairGenerateResult(new EdKeyPair(domainParameters.CurveE.Encode(Q), k));
        }

        public EdKeyPairValidateResult ValidateKeyPair(EdDomainParameters domainParameters, EdKeyPair keyPair)
        {
            // If Q is not a valid point on the specific curve, invalid
            EdPoint Q;
            try
            {
                Q = domainParameters.CurveE.Decode(keyPair.PublicQ);
            }
            catch (Exception e)     // the exception will be raised because square root cannot be taken
            {
                return new EdKeyPairValidateResult("Q does not lie on the curve");
            }

            // If Q == (0, 1), invalid
            if (Q.Equals(new EdPoint(0, 1)))
            {
                return new EdKeyPairValidateResult("Q cannot be neutral element");
            }

            // If Q is not a valid point on the specific curve, invalid
            // could make this more efficient
            if (!domainParameters.CurveE.PointExistsOnCurve(Q))
            {
                return new EdKeyPairValidateResult("Q does not lie on the curve");
            }

            // If n * Q == 0, valid
            // This is fast because the scalar (n) is taken modulo n... so it's 0
            if (domainParameters.CurveE.Multiply(Q, domainParameters.CurveE.OrderN).Equals(new EdPoint(0, 1)))
            {
                return new EdKeyPairValidateResult();
            }

            // Otherwise invalid
            return new EdKeyPairValidateResult("n * Q must equal (0, 1)");
        }

        public EdSignatureResult Sign(EdDomainParameters domainParameters, EdKeyPair keyPair, BitString message, bool skipHash = false)
        {
            // 1. Hash the private key
            Sha = domainParameters.Hash;
            // 912 is the output length for Ed448 when using SHAKE. It will not affect SHA512 output length for Ed25519.
            var h = Sha.HashMessage(new BitString(keyPair.PrivateD, domainParameters.CurveE.VariableB), 912).Digest;
            var prefix = h.Substring(0, domainParameters.CurveE.VariableB);
            // prune a as before. probably a better way to repeat these steps
            var a = BitString.ReverseByteOrder(h.MSBSubstring(0, domainParameters.CurveE.VariableB));
            for (int i = 0; i < domainParameters.CurveE.VariableC; i++)
            {
                a.Bits.Set(i, false);
            }
            for (int i = domainParameters.CurveE.VariableN; i < a.Bits.Length; i++)
            {
                a.Bits.Set(i, false);
            }

            // 2. Compute r
            // need to use dom4 if ed448
            var dom4 = domainParameters.CurveE.CurveName == Common.Asymmetric.DSA.Ed.Enums.Curve.Ed448 ? Dom4(0) : new BitString("");
            var rBits = Sha.HashMessage(BitString.ConcatenateBits(dom4, BitString.ConcatenateBits(prefix, message)), 912).Digest;
            rBits = BitString.ReverseByteOrder(rBits);
            var r = rBits.ToPositiveBigInteger() % domainParameters.CurveE.OrderN;

            // 3. Compute [r]G. R is the encoding of [r]G
            var rG = domainParameters.CurveE.Multiply(domainParameters.CurveE.BasePointG, r);
            var R = domainParameters.CurveE.Encode(rG);

            // 4. Define S
            // need to use dom4 if ed448
            var hash = Sha.HashMessage(BitString.ConcatenateBits(dom4, BitString.ConcatenateBits(new BitString(R, domainParameters.CurveE.VariableB), BitString.ConcatenateBits(new BitString(keyPair.PublicQ, domainParameters.CurveE.VariableB), message))), 912).Digest;
            var hashInt = BitString.ReverseByteOrder(hash).ToPositiveBigInteger() % domainParameters.CurveE.OrderN;
            var s = NumberTheory.Pow2(domainParameters.CurveE.VariableN) + a.ToPositiveBigInteger();
            var Sint = (r + (hashInt * s)).PosMod(domainParameters.CurveE.OrderN);
            // Encode S in little endian
            var S = BitString.ReverseByteOrder(new BitString(Sint, domainParameters.CurveE.VariableB));

            // 5. Form the signature by concatenating R and S
            var sig = BitString.ConcatenateBits(new BitString(R), S);
            return new EdSignatureResult(new EdSignature(sig.ToPositiveBigInteger()));
        }

        public EdVerificationResult Verify(EdDomainParameters domainParameters, EdKeyPair keyPair, BitString message, EdSignature signature, bool skipHash = false)
        {
            throw new NotImplementedException();
        }

        // Both secret generation methods exist, but we don't have a reason to use them. No need to worry about them.
        // These won't work well when the d value is provided via entropy provider instead of randomly generated because 
        // what comes out of the entropy provider is modified before becoming d
        private BigInteger GetSecretViaExtraRandomBits(BigInteger N)
        {
            var bitLength = N.ExactBitLength();
            var c = _entropyProvider.GetEntropy(bitLength + 64).ToPositiveBigInteger();
            var d = (c % (N - 1)) + 1;
            return d;
        }

        private BigInteger GetSecretViaTestingCandidates(BigInteger N)
        {
            var bitLength = N.ExactBitLength();

            BigInteger c;
            do
            {
                c = _entropyProvider.GetEntropy(bitLength).ToPositiveBigInteger();
            } while (c > N - 2);

            var d = c + 1;
            return d;
        }

        // Helper functions
        private BitString Dom4(BigInteger f, BitString c = null)
        {
            const string NAME = "SigEd448";
            if (c == null)
            {
                c = new BitString(""); // by default
            }

            var nameBits = StringToHex(NAME);
            var fBits = new BitString(f, 8);
            var cBits = new BitString(c.BitLength / 8, 8);  // length in octets

            return BitString.ConcatenateBits(nameBits, BitString.ConcatenateBits(fBits, cBits));
        }

        private static BitString StringToHex(string words)
        {
            var ba = Encoding.ASCII.GetBytes(words);
            return new BitString(ba);
        }
    }
}
