using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Math;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Math.Helpers;
using System;
using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed.Helpers;

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

            // Generate random number d
            var d = _entropyProvider.GetEntropy(1, NumberTheory.Pow2(domainParameters.CurveE.VariableB) - 1);

            // 1. Hash the private key
            // 2. Prune the buffer
            // Both accomplished by this function
            var h = HashPrivate(domainParameters, new BitString(d, domainParameters.CurveE.VariableB)).Buffer;

            // 3. Determine s
            var s = NumberTheory.Pow2(domainParameters.CurveE.VariableN) + h.ToPositiveBigInteger();

            // 4. Compute Q such that Q = s * G
            var Q = domainParameters.CurveE.Multiply(domainParameters.CurveE.BasePointG, s);

            // Encode Q
            var qEncoded = EdPointEncoder.Encode(Q, domainParameters.CurveE.VariableB);

            // Return key pair (Q, d)
            return new EdKeyPairGenerateResult(new EdKeyPair(qEncoded, new BitString(d, domainParameters.CurveE.VariableB)));
        }

        public EdKeyPairValidateResult ValidateKeyPair(EdDomainParameters domainParameters, EdKeyPair keyPair)
        {
            // If D is out of bounds, reject
            if (keyPair.PrivateD.ToPositiveBigInteger() < 1 || keyPair.PrivateD.ToPositiveBigInteger() > NumberTheory.Pow2(domainParameters.CurveE.VariableB) - 1)
            {
                return new EdKeyPairValidateResult("D must be able to be a b-bit string");
            }

            EdPoint Q;
            try
            {
                Q = domainParameters.CurveE.Decode(keyPair.PublicQ);
            }
            catch (Exception e)
            {
                return new EdKeyPairValidateResult(e.Message);
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

        public EdSignatureResult Sign(EdDomainParameters domainParameters, EdKeyPair keyPair, BitString message, bool preHash = false)
        {
            return Sign(domainParameters, keyPair, message, null, preHash);
        }

        public EdSignatureResult Sign(EdDomainParameters domainParameters, EdKeyPair keyPair, BitString message, BitString context, bool preHash = false)
        {
            Sha = domainParameters.Hash;

            // If preHash version, then the message becomes the hash of the message
            if (preHash)
            {
                message = Sha.HashMessage(message, 512).Digest;
            }

            // 1. Hash the private key
            var hashResult = HashPrivate(domainParameters, keyPair.PrivateD);

            // 2. Compute r
            // Determine dom. Empty if ed25519. Different for preHash function
            BitString dom;
            if (preHash)
            {
                dom = domainParameters.CurveE.CurveName == Curve.Ed448 ? Dom4(1, context) : Dom2(1, context);
            }
            else
            {
                dom = domainParameters.CurveE.CurveName == Curve.Ed448 ? Dom4(0, context) : new BitString("");
            }

            // Hash (dom4 || Prefix || message)
            var rBits = Sha.HashMessage(BitString.ConcatenateBits(dom, BitString.ConcatenateBits(hashResult.HDigest2, message)), 912).Digest;

            // Convert rBits to little endian and mod order n
            var r = BitString.ReverseByteOrder(rBits).ToPositiveBigInteger() % domainParameters.CurveE.OrderN;

            // 3. Compute [r]G. R is the encoding of [r]G
            var rG = domainParameters.CurveE.Multiply(domainParameters.CurveE.BasePointG, r);
            
            // Encode the point rG into a b-bit bitstring
            var R = domainParameters.CurveE.Encode(rG);

            // 4. Define S
            // Hash (dom4 || R || Q || M). Need to use dom4 if ed448
            var hashData = BitString.ConcatenateBits(keyPair.PublicQ, message);
            hashData = BitString.ConcatenateBits(dom, BitString.ConcatenateBits(R, hashData));
            var hash = Sha.HashMessage(hashData, 912).Digest;

            // Convert hash to int from little endian and mod order n
            var hashInt = BitString.ReverseByteOrder(hash).ToPositiveBigInteger() % domainParameters.CurveE.OrderN;

            // Determine s as done in key generation
            var s = NumberTheory.Pow2(domainParameters.CurveE.VariableN) + hashResult.Buffer.ToPositiveBigInteger();

            // Calculate S as an BigInteger
            var Sint = (r + (hashInt * s)).PosMod(domainParameters.CurveE.OrderN);

            // Encode S in little endian
            var S = BitString.ReverseByteOrder(new BitString(Sint, domainParameters.CurveE.VariableB));

            // 5. Form the signature by concatenating R and S
            return new EdSignatureResult(new EdSignature(R, S));
        }

        public EdVerificationResult Verify(EdDomainParameters domainParameters, EdKeyPair keyPair, BitString message, EdSignature signature, bool preHash = false)
        {
            return Verify(domainParameters, keyPair, message, signature, null, preHash);
        }

        public EdVerificationResult Verify(EdDomainParameters domainParameters, EdKeyPair keyPair, BitString message, EdSignature signature, BitString context, bool preHash = false)
        {
            Sha = domainParameters.Hash;

            // If preHash version, then the message becomes the hash of the message
            if (preHash)
            {
                message = Sha.HashMessage(message, 512).Digest;
            }

            // 1. Decode R, s, and Q
            EdPoint R;
            BigInteger s;
            EdPoint Q;
            try
            {
                var sigDecoded = SignatureDecoderHelper.DecodeSig(domainParameters, signature);
                R = sigDecoded.R;
                s = sigDecoded.s;
                Q = domainParameters.CurveE.Decode(keyPair.PublicQ);
            }
            catch (Exception e)
            {
                return new EdVerificationResult(e.Message);
            }

            // 2. Concatenate R || Q || M
            var hashData = BitString.ConcatenateBits(domainParameters.CurveE.Encode(R), BitString.ConcatenateBits(keyPair.PublicQ, message));

            // 3. Compute t
            // Determine dom. Empty if ed25519. Different for preHash function
            BitString dom;
            if (preHash)
            {
                dom = domainParameters.CurveE.CurveName == Curve.Ed448 ? Dom4(1, context) : Dom2(1, context);
            }
            else
            {
                dom = domainParameters.CurveE.CurveName == Curve.Ed448 ? Dom4(0, context) : new BitString("");
            }

            // Compute Hash(dom4 || HashData)
            var hash = Sha.HashMessage(BitString.ConcatenateBits(dom, hashData), 912).Digest;

            // Interpret hash as a little endian integer
            var t = BitString.ReverseByteOrder(hash).ToPositiveBigInteger();

            // 4. Check the verification equation [2^c * s]G = [2^c]R + [2^c * t]Q
            // 2^c
            var powC = NumberTheory.Pow2(domainParameters.CurveE.VariableC);

            // [2^c * s]G
            var lhs = domainParameters.CurveE.Multiply(domainParameters.CurveE.BasePointG, (powC * s).PosMod(domainParameters.CurveE.OrderN));

            // [2^c]R
            var rhs1 = domainParameters.CurveE.Multiply(R, powC);

            // [2^c * t]Q
            var rhs2 = domainParameters.CurveE.Multiply(Q, (powC * t).PosMod(domainParameters.CurveE.OrderN));

            // [2^c]R + [2^c * t]Q
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
        private BitString Dom2(BigInteger f, BitString c)
        {
            if (c == null)
            {
                c = new BitString(""); // by default
            }

            // "SigEd25519 no Ed25519 collisions" as hex
            var nameBits = new BitString("53696745643235353139206E6F204564323535313920636F6C6C6973696F6E73");
            var fBits = new BitString(f, 8);
            var cBits = new BitString(c.BitLength / 8, 8);  // length of c in octets
            cBits = BitString.ConcatenateBits(cBits, c);    // concatenate c

            return BitString.ConcatenateBits(nameBits, BitString.ConcatenateBits(fBits, cBits));
        }

        private BitString Dom4(BigInteger f, BitString c)
        {
            if (c == null)
            {
                c = new BitString(""); // by default
            }

            // "SigEd448" as hex
            var nameBits = new BitString("5369674564343438");
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
        private (BitString Buffer, BitString HDigest2) HashPrivate(EdDomainParameters domainParameters, BitString d)
        {
            // 912 is the output length for Ed448 when using SHAKE. It will not affect SHA512 output length for Ed25519.
            var h = Sha.HashMessage(d, 912).Digest;

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
    }
}
