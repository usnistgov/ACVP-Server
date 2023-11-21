using System;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.LMS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.LMS.Native.Keys;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.LMS.Native
{
    public class Lms : ILmsSigner, ILmsVerifier
    {
        private readonly ILmOtsKeyPairFactory _lmOtsKeyPairFactory;
        private readonly ILmOtsSigner _lmOtsSigner;
        private readonly IShaFactory _shaFactory;

        public Lms(ILmOtsKeyPairFactory lmOtsKeyPairFactory, ILmOtsSigner lmOtsSigner, IShaFactory shaFactory)
        {
            _lmOtsKeyPairFactory = lmOtsKeyPairFactory;
            _lmOtsSigner = lmOtsSigner;
            _shaFactory = shaFactory;
        }

        public ILmsSignatureResult Sign(ILmsPrivateKey privateKey, ILmOtsRandomizerC randomizerC, byte[] message)
        {
            var sha = LmsHelpers.GetSha(_shaFactory, privateKey.LmsAttribute.Mode);

            var potentialQ = privateKey.GetQ();
            if (!potentialQ.HasValue)
            {
                return new LmsSignatureResult();
            }

            var q = potentialQ.Value;

            // 2. Extract h from the typecode, according to Table 2.
            var h = privateKey.LmsAttribute.H;
            var r = (1 << h) + q;
            var path = new byte[h][];
            var buffer = new byte[AttributesHelper.GetBufferByteLengthBasedOnOneWayFunction(privateKey.LmsAttribute.Mode)];
            
            // 3. Create the LM-OTS signature for the message: ots_signature = lmots_sign(message, LMS_PRIV[q])
            // Reconstruct private key
            var lmOtsPrivateKey = _lmOtsKeyPairFactory.GetPrivateKey(privateKey.LmOtsAttribute, privateKey.I, q.GetBytes(), privateKey.Seed);
            var lmOtsSignature = _lmOtsSigner.Sign(lmOtsPrivateKey, randomizerC, message);
            var i = 0;

            /*
			 4. Compute the array path as follows:
		        i = 0
		        r = 2^h + q
		        while (i < h) {
		          temp = (r / 2^i) xor 1
		          path[i] = T[temp]
		          i = i + 1
		        }
		    */
            while (i < h)
            {
                // temp = (r / 2^i) xor 1
                var temp = ((r / (1 << i)) ^ 1);
                // path[i] = T[temp]
                path[i] = CalculateT(sha, privateKey, temp, buffer);
                i++;
            }

            // 5. S = u32str(q) || ots_signature || u32str(type) || path[0] || path[1] || ... || path[h-1]
            var s = new byte[4 + lmOtsSignature.Length + 4 + privateKey.LmsAttribute.H * privateKey.LmsAttribute.M];
            Array.Copy(q.GetBytes(), 0, s, 0, 4);
            Array.Copy(lmOtsSignature, 0, s, 4, lmOtsSignature.Length);
            Array.Copy(privateKey.LmsAttribute.NumericIdentifier, 0, s, 4 + lmOtsSignature.Length, 4);
            var pathStartIndex = 4 + lmOtsSignature.Length + 4;
            for (i = 0; i < privateKey.LmsAttribute.H; i++)
            {
                Array.Copy(path[i], 0, s, pathStartIndex + (i * privateKey.LmsAttribute.M), privateKey.LmsAttribute.M);
            }

            return new LmsSignatureResult(s);
        }

        private byte[] CalculateT(ISha sha, ILmsPrivateKey privateKey, int r, byte[] buffer)
        {
            var powTwoTreeHeight = 1 << privateKey.LmsAttribute.H;
            if (r >= powTwoTreeHeight)
            {
                // H(I||u32str(r)||u16str(D_LEAF)||OTS_PUB_HASH[r-2^h])
                
                // If we already have it stored, just return it
                if (privateKey.HasPrecomputedHash(r))
                {
                    return privateKey.GetTreeNodeAtIndex(r);
                }
                
                // Guaranteed to be a final leaf of the tree
                sha.Init();
                sha.Update(privateKey.I, privateKey.I.BitLength());
                sha.Update(r, 32);
                sha.Update(LmsHelpers.D_LEAF, 16);
                
                var otsPublicKeyHash = _lmOtsKeyPairFactory.GetKeyPair(privateKey.LmOtsAttribute.Mode, privateKey.I, (r - powTwoTreeHeight).GetBytes(), privateKey.Seed).PublicKey.K;
                
                sha.Update(otsPublicKeyHash, otsPublicKeyHash.BitLength());
                sha.Final(buffer, buffer.BitLength());
                
                var result = new byte[privateKey.LmsAttribute.M];
                Array.Copy(buffer, result, result.Length);
                
                return result;
            }
            else
            {
                // H(I||u32str(r)||u16str(D_INTR)||T[2*r]||T[2*r+1])
                // T is an intermediate node, check if we already have it computed
                var t1 = privateKey.HasPrecomputedHash(2 * r) ? privateKey.GetTreeNodeAtIndex(2 * r) : CalculateT(sha, privateKey, 2 * r, buffer);
                var t2 = privateKey.HasPrecomputedHash(2 * r + 1) ? privateKey.GetTreeNodeAtIndex(2 * r + 1) : CalculateT(sha, privateKey, 2 * r + 1, buffer);

                sha.Init();
                sha.Update(privateKey.I, privateKey.I.BitLength());
                sha.Update(r, 32);
                sha.Update(LmsHelpers.D_INTR, 16);
                sha.Update(t1, t1.BitLength());
                sha.Update(t2, t2.BitLength());
                sha.Final(buffer, buffer.BitLength());
                
                var result = new byte[privateKey.LmsAttribute.M];
                Array.Copy(buffer, result, result.Length);
                
                return result;
            }
        }

        public LmsVerificationResult Verify(byte[] lmsPublicKey, byte[] signature, byte[] message)
        //public LmsVerificationResult Verify(ILmsPublicKey lmsPublicKey, byte[] signature, byte[] message)
        {
            // 1. If the public key is not at least eight bytes long, return INVALID.
            if (lmsPublicKey.Length < 8)
            {
                return new LmsVerificationResult($"{nameof(lmsPublicKey)} must be at least 8 bytes.");
            }

            // 2a. pubtype = strTou32(first 4 bytes of public key)
            var lmsType = AttributesHelper.GetLmsModeFromTypeCode(lmsPublicKey.Take(4).ToArray());

            if (lmsType == LmsMode.Invalid)
            {
                return new LmsVerificationResult($"Bad format to public key, could not parse LMS type.");
            }

            var lmsAttribute = AttributesHelper.GetLmsAttribute(lmsType);

            // 2b. ots_typecode = strTou32(next 4 bytes of public key)
            var lmOtsType = AttributesHelper.GetLmOtsModeFromTypeCode(lmsPublicKey.Skip(4).Take(4).ToArray());

            if (lmOtsType == LmOtsMode.Invalid)
            {
                return new LmsVerificationResult($"Bad format to public key, could not parse LM-OTS type.");
            }

            // 2c. Set m according to pubtype, based on Table 2.
            var lmOtsAttribute = AttributesHelper.GetLmOtsAttribute(lmOtsType);

            // 2d. If the public key is not exactly 24 + m bytes long, return INVALID.
            var expectedPublicKeyBytesLength = 24 + lmsAttribute.M;
            if (lmsPublicKey.Length != expectedPublicKeyBytesLength)
            {
                return new LmsVerificationResult($"Expected LMS public key to be exactly {expectedPublicKeyBytesLength} length, was {lmsPublicKey.Length}.");
            }

            // 2e. I = next 16 bytes of the public key
            // lmsPublicKey[8] ... lmsPublicKey[23] is "I" 
            var i = lmsPublicKey.Skip(4 + 4).Take(16).ToArray();

            // f. T[1] = next m bytes of the public key
            // lmsPublicKey[24] ... lmsPublicKey[24+m] is the rolled up public key of the merkle tree
            var t = lmsPublicKey.Skip(4 + 4 + 16).Take(lmsAttribute.M).ToArray();

            // 3. Compute the LMS Public Key Candidate Tc from the signature,
            // message, identifier, pubtype, and ots_typecode, using
            //	Algorithm 6a.
            var lmsPublicKeyWithModes = new LmsPublicKey(lmsPublicKey);
            var tCandidate = GetCandidateT(lmsPublicKeyWithModes, lmOtsAttribute, i, signature, message);

            // 4. If Tc is equal to T[1], return VALID; otherwise, return INVALID.
            return t.SequenceEqual(tCandidate) ? new LmsVerificationResult() : new LmsVerificationResult("T candidate did not match provided value");
        }

        private byte[] GetCandidateT(ILmsPublicKey lmsPublicKey, LmOtsAttribute lmOtsAttribute, byte[] i, byte[] signature, byte[] message)
        {
            // 1. If the signature is not at least eight bytes long, return INVALID.
            if (signature.Length < 8)
            {
                throw new ArgumentException($"Signature must be at least 8 bytes.");
            }

            // 2a. q = strTou32(first 4 bytes of signature)
            var q = signature.Take(4).ToArray();
            var qNum = (int) new BitString(q).ToPositiveBigInteger();

            // b. otssigtype = strTou32(next 4 bytes of signature)
            var otsSigTypeCode = signature.Skip(4).Take(4).ToArray();
            var otsSigMode = AttributesHelper.GetLmOtsModeFromTypeCode(otsSigTypeCode);
            var otsSigAttribute = AttributesHelper.GetLmOtsAttribute(otsSigMode);

            // c. If otssigtype is not the OTS typecode from the public key, return INVALID.
            if (otsSigAttribute.Mode != lmOtsAttribute.Mode)
            {
                throw new ArgumentException($"Signature LM-OTS mode did not match public key LM-OTS mode.");
            }

            var sigLengthMinimum = 12 + otsSigAttribute.N * (otsSigAttribute.P + 1);
            if (signature.Length < sigLengthMinimum)
            {
                throw new ArgumentException($"Signature needs to be at least {sigLengthMinimum} bytes, was {signature.Length}");
            }

            // e. lmots_signature = bytes 4 through 7 + n * (p + 1) of signature
            var lmOtsSignature = signature.Skip(4).Take(4 + lmOtsAttribute.N * (lmOtsAttribute.P + 1)).ToArray();

            // f. sigtype = strTou32(bytes 8 + n * (p + 1)) through 11 + n * (p + 1) of signature)
            var sigType = signature.Skip(8 + lmOtsAttribute.N * (lmOtsAttribute.P + 1)).Take(4).ToArray();
            var sigTypeMode = AttributesHelper.GetLmsModeFromTypeCode(sigType);

            // g. If sigtype is not the LM typecode from the public key, return INVALID.
            if (sigTypeMode != lmsPublicKey.LmsAttribute.Mode)
            {
                throw new ArgumentException($"Signature LMS mode did not match public key LMS mode.");
            }

            // i. If q >= 2^h or the signature is not exactly 12 + n * (p + 1) + m * h bytes long, return INVALID.
            if (qNum > (1 << lmsPublicKey.LmsAttribute.H))
            {
                throw new ArgumentException("Signature's parsed q was greater than 2^h");
            }

            var sigExpectedLength = 12 + lmOtsAttribute.N * (lmOtsAttribute.P + 1) + lmsPublicKey.LmsAttribute.M * lmsPublicKey.LmsAttribute.H;
            if (signature.Length != sigExpectedLength)
            {
                throw new ArgumentException($"Signature expected to be {sigExpectedLength} bytes, was {signature.Length}");
            }

            /*
			 j. Set path as follows:
			             path[0] = next m bytes of signature
			             path[1] = next m bytes of signature
			                ...
			           path[h-1] = next m bytes of signature
			 */
            var pathStartIndex = 8 + lmOtsAttribute.N * (lmOtsAttribute.P + 1) + 4;
            var paths = new byte[lmsPublicKey.LmsAttribute.H][];
            for (var j = 0; j < lmsPublicKey.LmsAttribute.H; j++)
            {
                paths[j] = signature
                    .Skip(pathStartIndex + (j * lmsPublicKey.LmsAttribute.M))
                    .Take(lmsPublicKey.LmsAttribute.M).ToArray();
            }

            // 3. Kc = candidate public key computed by applying Algorithm 4b
            // to the signature lmots_signature, the message, and the identifiers I, q
            var sha = LmsHelpers.GetSha(_shaFactory, lmOtsAttribute.Mode);
            var otsPublicKeyCandidate = LmsHelpers.GetLmOtsPublicKeyCandidate(sha, lmOtsAttribute, lmOtsSignature, message, i, q);

            // node_num = 2^h + q
            var nodeNumber = (1 << lmsPublicKey.LmsAttribute.H) + qNum;

            var bufferSize = AttributesHelper.GetBufferByteLengthBasedOnOneWayFunction(lmsPublicKey.LmsAttribute.Mode);
            var buffer = new byte[bufferSize];

            //tmp = H(I || u32str(node_num) || u16str(D_LEAF) || Kc)
            var temp = new byte[lmsPublicKey.LmsAttribute.M];
            sha.Init();
            sha.Update(i, i.BitLength());
            sha.Update(nodeNumber, 32);
            sha.Update(LmsHelpers.D_LEAF, 16);
            sha.Update(otsPublicKeyCandidate, otsPublicKeyCandidate.BitLength());
            sha.Final(buffer, buffer.BitLength());

            Array.Copy(buffer, temp, temp.Length);

            // i = 0
            var ii = 0;
            while (nodeNumber > 1)
            {
                //if (node_num is odd):
                if (nodeNumber % 2 == 1)
                {
                    // tmp = H(I||u32str(node_num/2)||u16str(D_INTR)||path[i]||tmp)
                    sha.Init();
                    sha.Update(i, i.BitLength());
                    sha.Update((nodeNumber / 2), 32);
                    sha.Update(LmsHelpers.D_INTR, 16);
                    sha.Update(paths[ii], paths[ii].BitLength());
                    sha.Update(temp, temp.BitLength());
                    sha.Final(buffer, buffer.BitLength());

                    Array.Copy(buffer, temp, temp.Length);
                }
                else
                {
                    //tmp = H(I||u32str(node_num/2)||u16str(D_INTR)||tmp||path[i])
                    sha.Init();
                    sha.Update(i, i.BitLength());
                    sha.Update((nodeNumber / 2), 32);
                    sha.Update(LmsHelpers.D_INTR, 16);
                    sha.Update(temp, temp.BitLength());
                    sha.Update(paths[ii], paths[ii].BitLength());
                    sha.Final(buffer, buffer.BitLength());

                    Array.Copy(buffer, temp, temp.Length);
                }

                //node_num = node_num/2
                nodeNumber /= 2;
                //i = i + 1
                ii++;
            }

            // 5. Return Tc.
            return temp;
        }
    }
}
