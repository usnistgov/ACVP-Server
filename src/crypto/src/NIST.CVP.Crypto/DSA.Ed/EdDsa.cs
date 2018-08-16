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
            Sha = domainParameters.Hash;

            // Generate random number k ... not sure what the range should be for this.
            var k = _entropyProvider.GetEntropy(1, NumberTheory.Pow2(domainParameters.CurveE.VariableB + 1) - 1);

            // 1. Hash the private key
            // 2. Prune the buffer
            // Both accomplished by this function
            var h = HashPrivate(domainParameters, k).Buffer;

            // 3. Determine s
            var s = NumberTheory.Pow2(domainParameters.CurveE.VariableN) + h.ToPositiveBigInteger();

            // 4. Compute Q such that Q = s * G
            var Q = domainParameters.CurveE.Multiply(domainParameters.CurveE.BasePointG, s);

            // Return key pair (Q, d)
            return new EdKeyPairGenerateResult(new EdKeyPair(domainParameters.CurveE.Encode(Q), k));
        }

        public EdKeyPairValidateResult ValidateKeyPair(EdDomainParameters domainParameters, EdKeyPair keyPair)
        {
            Sha = domainParameters.Hash;

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
            return Sign(domainParameters, keyPair, message, null, skipHash);
        }

        public EdSignatureResult Sign(EdDomainParameters domainParameters, EdKeyPair keyPair, BitString message, BitString context, bool skipHash = false)
        {
            Sha = domainParameters.Hash;

            // 1. Hash the private key
            var hashResult = HashPrivate(domainParameters, keyPair.PrivateD);

            // 2. Compute r
            // Determine dom4. Empty if ed25519
            var dom4 = domainParameters.CurveE.CurveName == Common.Asymmetric.DSA.Ed.Enums.Curve.Ed448 ? Dom4(0, context) : new BitString("");

            // Hash (dom4 || Prefix || message)
            var rBits = Sha.HashMessage(BitString.ConcatenateBits(dom4, BitString.ConcatenateBits(hashResult.HDigest2, message)), 912).Digest;

            // Convert rBits to little endian and mod order n
            var r = BitString.ReverseByteOrder(rBits).ToPositiveBigInteger() % domainParameters.CurveE.OrderN;

            // 3. Compute [r]G. R is the encoding of [r]G
            var rG = domainParameters.CurveE.Multiply(domainParameters.CurveE.BasePointG, r);
            
            // Encode the point rG into a b-bit bitstring
            var R = domainParameters.CurveE.Encode(rG);

            // 4. Define S
            // Hash (dom4 || R || Q || B). Need to use dom4 if ed448
            var preHash = BitString.ConcatenateBits(new BitString(keyPair.PublicQ, domainParameters.CurveE.VariableB), message);
            preHash = BitString.ConcatenateBits(dom4, BitString.ConcatenateBits(new BitString(R, domainParameters.CurveE.VariableB), preHash));
            var hash = Sha.HashMessage(preHash, 912).Digest;

            // Convert hash to int from little endian and mod order n
            var hashInt = BitString.ReverseByteOrder(hash).ToPositiveBigInteger() % domainParameters.CurveE.OrderN;

            // Determine s as done in key generation
            var s = NumberTheory.Pow2(domainParameters.CurveE.VariableN) + hashResult.Buffer.ToPositiveBigInteger();

            // Calculate S as an BigInteger
            var Sint = (r + (hashInt * s)).PosMod(domainParameters.CurveE.OrderN);

            // Encode S in little endian
            var S = BitString.ReverseByteOrder(new BitString(Sint, domainParameters.CurveE.VariableB));

            // 5. Form the signature by concatenating R and S
            var sig = BitString.ConcatenateBits(new BitString(R), S);
            return new EdSignatureResult(new EdSignature(sig.ToPositiveBigInteger()));
        }

        public EdVerificationResult Verify(EdDomainParameters domainParameters, EdKeyPair keyPair, BitString message, EdSignature signature, bool skipHash = false)
        {
            return Verify(domainParameters, keyPair, message, signature, null, skipHash);
        }

        public EdVerificationResult Verify(EdDomainParameters domainParameters, EdKeyPair keyPair, BitString message, EdSignature signature, BitString context, bool skipHash = false)
        {
            Sha = domainParameters.Hash;

            // 1. Decode R, s, and Q
            EdPoint R;
            BigInteger s;
            EdPoint Q;
            try
            {
                var sigDecoded = DecodeSig(domainParameters, signature);
                R = sigDecoded.R;
                s = sigDecoded.s;
                Q = domainParameters.CurveE.Decode(keyPair.PublicQ);
            }
            catch (Exception e)
            {
                return new EdVerificationResult(e.Message);
            }

            // 2. Concatenate R || Q || M
            var hashData = BitString.ConcatenateBits(new BitString(domainParameters.CurveE.Encode(R)), BitString.ConcatenateBits(new BitString(keyPair.PublicQ), message));

            // 3. Compute t
            // Determine dom4. Empty if ed25519
            var dom4 = domainParameters.CurveE.CurveName == Common.Asymmetric.DSA.Ed.Enums.Curve.Ed448 ? Dom4(0, context) : new BitString("");

            // Compute Hash(dom4 || HashData)
            var hash = Sha.HashMessage(BitString.ConcatenateBits(dom4, hashData), 912).Digest;

            // Interpret hash as a little endian integer
            var t = BitString.ReverseByteOrder(hash).ToBigInteger();

            // 4. Check the verification equation [2^c * s]G = [2^c]R + (2^c * t)Q
            var powC = NumberTheory.Pow2(domainParameters.CurveE.VariableC);
            var lhs = domainParameters.CurveE.Multiply(domainParameters.CurveE.BasePointG, (powC * s).PosMod(domainParameters.CurveE.OrderN));
            var rhs1 = domainParameters.CurveE.Multiply(R, powC);
            var rhs2 = domainParameters.CurveE.Multiply(Q, (powC * t).PosMod(domainParameters.CurveE.OrderN));
            var rhs = domainParameters.CurveE.Add(rhs1, rhs2);

            if (lhs.Equals(rhs))
            {
                return new EdVerificationResult();
            }

            return new EdVerificationResult("The verification equation is not satisfied. Signature not valid");
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
        private BitString Dom4(BigInteger f, BitString c)
        {
            const string NAME = "SigEd448";
            if (c == null)
            {
                c = new BitString(""); // by default
            }

            var nameBits = StringToHex(NAME);
            var fBits = new BitString(f, 8);
            var cBits = new BitString(c.BitLength / 8, 8);  // length of c in octets
            cBits = BitString.ConcatenateBits(cBits, c);    // concatenate c

            return BitString.ConcatenateBits(nameBits, BitString.ConcatenateBits(fBits, cBits));
        }

        /// <summary>
        /// Hashs private key and formats both the prefix (used in signing) and A (used in generating the public key)
        /// </summary>
        /// <param name="domainParameters"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        private (BitString Buffer, BitString HDigest2) HashPrivate(EdDomainParameters domainParameters, BigInteger d)
        {
            // 912 is the output length for Ed448 when using SHAKE. It will not affect SHA512 output length for Ed25519.
            var h = Sha.HashMessage(new BitString(d, domainParameters.CurveE.VariableB), 912).Digest;

            // Split the hash result in half
            var hDigest2 = h.Substring(0, domainParameters.CurveE.VariableB);
            var buffer = BitString.ReverseByteOrder(h.MSBSubstring(0, domainParameters.CurveE.VariableB));

            // Prune the buffer
            for (int i = 0; i < domainParameters.CurveE.VariableC; i++)
            {
                buffer.Bits.Set(i, false);
            }
            for (int i = domainParameters.CurveE.VariableN; i < buffer.Bits.Length; i++)
            {
                buffer.Bits.Set(i, false);
            }

            return (buffer, hDigest2);
        }

        private (EdPoint R, BigInteger s) DecodeSig(EdDomainParameters domainParameters, EdSignature sig)
        {
            var rBits = new BitString(sig.Sig).MSBSubstring(0, domainParameters.CurveE.VariableB);
            var sBits = new BitString(sig.Sig).Substring(0, domainParameters.CurveE.VariableB);

            var R = domainParameters.CurveE.Decode(rBits.ToPositiveBigInteger());
            var s = BitString.ReverseByteOrder(sBits).ToPositiveBigInteger();

            return (R, s);
        } 

        private static BitString StringToHex(string words)
        {
            var ba = Encoding.ASCII.GetBytes(words);
            return new BitString(ba);
        }
    }
}
