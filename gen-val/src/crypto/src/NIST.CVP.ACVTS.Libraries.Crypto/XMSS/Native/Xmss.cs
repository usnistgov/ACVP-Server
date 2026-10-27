using System;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.XMSS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.XMSS.Native
{
    public class Xmss : IXmssSigner, IXmssVerifier
    {
        private readonly IWotsPlus _wotsPlus;
        private readonly IShaFactory _shaFactory;

        public Xmss(IWotsPlus wotsPlus, IShaFactory shaFactory)
        {
            _wotsPlus = wotsPlus;
            _shaFactory = shaFactory;
        }

        public IXmssSignatureResult Sign(IXmssPrivateKey privateKey, byte[] message)
        {
            var attribute = privateKey.XmssAttribute;
            var sha = XmssHelpers.GetSha(_shaFactory, attribute.Mode);
            var buffer = new byte[AttributesHelper.GetBufferByteLengthBasedOnOneWayFunction(attribute.Mode)];

            var potentialIdx = privateKey.GetIdx();
            if (!potentialIdx.HasValue)
            {
                return new XmssSignatureResult();
            }

            var idx = potentialIdx.Value;

            // r = PRF(SK_PRF, toByte(idx_sig, 32))
            var r = XmssHelpers.Prf(sha, attribute, privateKey.SkPrf, XmssHelpers.ToByte(idx, 32), buffer);

            var messageDigest = XmssHelpers.HashMessage(sha, attribute, r, privateKey.Root, idx, message, buffer);

            var otsAdrs = new uint[8];
            XmssHelpers.SetType(otsAdrs, XmssHelpers.AddressTypeOts);
            XmssHelpers.SetOtsAddress(otsAdrs, (uint)idx);
            var otsSignature = _wotsPlus.Sign(attribute, messageDigest, privateKey.SkSeed, privateKey.PubSeed, otsAdrs);

            /* The authentication path node at height i is the sibling of the leaf's ancestor at
               that height: index (idx / 2^i) xor 1 at its level, node 2^(h-i) + ((idx / 2^i) xor 1)
               of the one-indexed tree. */
            var auth = new byte[attribute.H][];
            for (var i = 0; i < attribute.H; i++)
            {
                var node = (1 << (attribute.H - i)) + ((idx >> i) ^ 1);
                auth[i] = CalculateNode(sha, privateKey, node, buffer);
            }

            // Sig = idx_sig || r || sig_ots || auth
            var signature = new byte[4 + attribute.N + otsSignature.Length + attribute.H * attribute.N];
            Array.Copy(idx.GetBytes(), 0, signature, 0, 4);
            Array.Copy(r, 0, signature, 4, attribute.N);
            Array.Copy(otsSignature, 0, signature, 4 + attribute.N, otsSignature.Length);
            var authStartIndex = 4 + attribute.N + otsSignature.Length;
            for (var i = 0; i < attribute.H; i++)
            {
                Array.Copy(auth[i], 0, signature, authStartIndex + i * attribute.N, attribute.N);
            }

            return new XmssSignatureResult(signature);
        }

        public XmssVerificationResult Verify(byte[] publicKey, byte[] signature, byte[] message)
        {
            if (publicKey == null || publicKey.Length < 4)
            {
                return new XmssVerificationResult($"{nameof(publicKey)} must be at least 4 bytes.");
            }

            var mode = AttributesHelper.GetXmssModeFromTypeCode(publicKey.Take(4).ToArray());
            if (mode == XmssMode.Invalid)
            {
                return new XmssVerificationResult("Bad format to public key, could not parse XMSS type.");
            }

            var attribute = AttributesHelper.GetXmssAttribute(mode);

            var expectedPublicKeyLength = 4 + 2 * attribute.N;
            if (publicKey.Length != expectedPublicKeyLength)
            {
                return new XmssVerificationResult(
                    $"Expected XMSS public key to be exactly {expectedPublicKeyLength} bytes, was {publicKey.Length}.");
            }

            var root = publicKey.Skip(4).Take(attribute.N).ToArray();
            var pubSeed = publicKey.Skip(4 + attribute.N).Take(attribute.N).ToArray();

            var expectedSignatureLength = 4 + attribute.N * (1 + attribute.Len + attribute.H);
            if (signature == null || signature.Length != expectedSignatureLength)
            {
                return new XmssVerificationResult(
                    $"Expected signature to be exactly {expectedSignatureLength} bytes, was {signature?.Length ?? 0}.");
            }

            var idx = (long)new BitString(signature.Take(4).ToArray()).ToPositiveBigInteger();
            if (idx >= 1L << attribute.H)
            {
                return new XmssVerificationResult("Signature's parsed idx_sig was beyond the height of the tree.");
            }

            var r = signature.Skip(4).Take(attribute.N).ToArray();
            var otsSignature = signature.Skip(4 + attribute.N).Take(attribute.Len * attribute.N).ToArray();

            var auth = new byte[attribute.H][];
            var authStartIndex = 4 + attribute.N + attribute.Len * attribute.N;
            for (var i = 0; i < attribute.H; i++)
            {
                auth[i] = signature.Skip(authStartIndex + i * attribute.N).Take(attribute.N).ToArray();
            }

            var sha = XmssHelpers.GetSha(_shaFactory, attribute.Mode);
            var buffer = new byte[AttributesHelper.GetBufferByteLengthBasedOnOneWayFunction(attribute.Mode)];

            var messageDigest = XmssHelpers.HashMessage(sha, attribute, r, root, idx, message, buffer);

            var otsAdrs = new uint[8];
            XmssHelpers.SetType(otsAdrs, XmssHelpers.AddressTypeOts);
            XmssHelpers.SetOtsAddress(otsAdrs, (uint)idx);
            var wotsPk = _wotsPlus.PkFromSig(attribute, otsSignature, messageDigest, pubSeed, otsAdrs);

            var ltreeAdrs = new uint[8];
            XmssHelpers.SetType(ltreeAdrs, XmssHelpers.AddressTypeLtree);
            XmssHelpers.SetLtreeAddress(ltreeAdrs, (uint)idx);
            var leaf = XmssHelpers.LTree(sha, attribute, XmssHelpers.Unflatten(attribute, wotsPk), pubSeed, ltreeAdrs,
                buffer);

            var nodeAdrs = new uint[8];
            XmssHelpers.SetType(nodeAdrs, XmssHelpers.AddressTypeHashTree);
            var rootCandidate = XmssHelpers.ComputeRoot(sha, attribute, leaf, (uint)idx, auth, pubSeed, nodeAdrs,
                buffer);

            return rootCandidate.SequenceEqual(root)
                ? new XmssVerificationResult()
                : new XmssVerificationResult("Root candidate did not match provided value");
        }

        /// <summary>
        /// Compute the value of the one-indexed tree node <see cref="r"/>, where index 1 is the
        /// root and indices 2^h .. 2^(h+1) - 1 are the leaves, preferring the private key's
        /// precomputed nodes over recomputation.
        /// </summary>
        private byte[] CalculateNode(ISha sha, IXmssPrivateKey privateKey, int r, byte[] buffer)
        {
            if (privateKey.HasPrecomputedHash(r))
            {
                return privateKey.GetTreeNodeAtIndex(r);
            }

            var attribute = privateKey.XmssAttribute;
            var powTwoTreeHeight = 1 << attribute.H;

            if (r >= powTwoTreeHeight)
            {
                // A leaf: the L-tree compression of the WOTS+ public key at this index.
                return GenLeaf(sha, attribute, privateKey.SkSeed, privateKey.PubSeed, (uint)(r - powTwoTreeHeight),
                    buffer);
            }

            var left = CalculateNode(sha, privateKey, 2 * r, buffer);
            var right = CalculateNode(sha, privateKey, 2 * r + 1, buffer);

            var levelFromTop = XmssHelpers.FloorLog2(r);
            var childHeight = attribute.H - levelFromTop - 1;
            var indexAtLevel = r - (1 << levelFromTop);

            var nodeAdrs = new uint[8];
            XmssHelpers.SetType(nodeAdrs, XmssHelpers.AddressTypeHashTree);
            XmssHelpers.SetTreeHeight(nodeAdrs, (uint)childHeight);
            XmssHelpers.SetTreeIndex(nodeAdrs, (uint)indexAtLevel);

            var concatenated = new byte[2 * attribute.N];
            Array.Copy(left, 0, concatenated, 0, attribute.N);
            Array.Copy(right, 0, concatenated, attribute.N, attribute.N);

            return XmssHelpers.ThashH(sha, attribute, concatenated, privateKey.PubSeed, nodeAdrs, buffer);
        }

        /// <summary>
        /// Compute a leaf node: the WOTS+ public key at index <see cref="leafIndex"/> compressed
        /// with an L-tree.
        /// </summary>
        private byte[] GenLeaf(ISha sha, XmssAttribute attribute, byte[] skSeed, byte[] pubSeed, uint leafIndex,
            byte[] buffer)
        {
            var otsAdrs = new uint[8];
            XmssHelpers.SetType(otsAdrs, XmssHelpers.AddressTypeOts);
            XmssHelpers.SetOtsAddress(otsAdrs, leafIndex);
            var wotsPk = _wotsPlus.PkGen(attribute, skSeed, pubSeed, otsAdrs);

            var ltreeAdrs = new uint[8];
            XmssHelpers.SetType(ltreeAdrs, XmssHelpers.AddressTypeLtree);
            XmssHelpers.SetLtreeAddress(ltreeAdrs, leafIndex);
            return XmssHelpers.LTree(sha, attribute, XmssHelpers.Unflatten(attribute, wotsPk), pubSeed, ltreeAdrs,
                buffer);
        }
    }
}
