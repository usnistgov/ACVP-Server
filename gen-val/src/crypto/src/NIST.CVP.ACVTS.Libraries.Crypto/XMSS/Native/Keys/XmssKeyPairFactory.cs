using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.XMSS.Native.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.XMSS.Native.Keys
{
    public class XmssKeyPairFactory : IXmssKeyPairFactory
    {
        private readonly IWotsPlus _wotsPlus;
        private readonly IShaFactory _shaFactory;

        public XmssKeyPairFactory(IWotsPlus wotsPlus, IShaFactory shaFactory)
        {
            _wotsPlus = wotsPlus;
            _shaFactory = shaFactory;
        }

        public IXmssKeyPair GetKeyPair(XmssMode xmssMode, byte[] seed, int x = 0)
        {
            var attribute = AttributesHelper.GetXmssAttribute(xmssMode);

            var expectedSeedLength = 3 * attribute.N;
            if (seed == null || seed.Length != expectedSeedLength)
            {
                throw new ArgumentException($"{nameof(seed)} was expected to be {expectedSeedLength} bytes.");
            }

            if (x < 0 || x > attribute.H)
            {
                throw new ArgumentException($"{nameof(x)} must be within [0, {attribute.H}].");
            }

            var skSeed = new byte[attribute.N];
            var skPrf = new byte[attribute.N];
            var pubSeed = new byte[attribute.N];
            Array.Copy(seed, 0, skSeed, 0, attribute.N);
            Array.Copy(seed, attribute.N, skPrf, 0, attribute.N);
            Array.Copy(seed, 2 * attribute.N, pubSeed, 0, attribute.N);

            byte[][] storedNodes = null;
            if (x > 0)
            {
                storedNodes = new byte[2 << x][];
                // so we can safely write to JSON later on for Pools
                storedNodes[0] = Array.Empty<byte>();
            }

            var root = ComputeTree(attribute, skSeed, pubSeed, storedNodes);

            var privateKey = new XmssPrivateKey(attribute, skSeed, skPrf, pubSeed, root, 0, x, storedNodes);
            var publicKey = new XmssPublicKey(attribute, root, pubSeed);

            return new XmssKeyPair
            {
                XmssAttribute = attribute,
                PrivateKey = privateKey,
                PublicKey = publicKey
            };
        }

        /// <summary>
        /// Compute the full tree with the treehash algorithm of
        /// https://datatracker.ietf.org/doc/html/rfc8391#section-4.1.6, storing any node whose
        /// one-indexed tree index falls within <see cref="storedNodes"/>.
        /// </summary>
        /// <returns>The n-byte root node.</returns>
        private byte[] ComputeTree(XmssAttribute attribute, byte[] skSeed, byte[] pubSeed, byte[][] storedNodes)
        {
            var sha = XmssHelpers.GetSha(_shaFactory, attribute.Mode);
            var buffer = new byte[AttributesHelper.GetBufferByteLengthBasedOnOneWayFunction(attribute.Mode)];

            var stack = new byte[attribute.H + 1][];
            var heights = new int[attribute.H + 1];
            var offset = 0;
            var storeLimit = storedNodes?.Length ?? 0;
            var leafCount = 1 << attribute.H;

            for (var idx = 0; idx < leafCount; idx++)
            {
                var leaf = GenLeaf(sha, attribute, skSeed, pubSeed, (uint)idx, buffer);
                stack[offset] = leaf;
                heights[offset] = 0;
                offset++;

                if (leafCount + idx < storeLimit)
                {
                    storedNodes[leafCount + idx] = leaf;
                }

                while (offset >= 2 && heights[offset - 1] == heights[offset - 2])
                {
                    var childHeight = heights[offset - 1];
                    var treeIndex = idx >> (childHeight + 1);

                    var nodeAdrs = new uint[8];
                    XmssHelpers.SetType(nodeAdrs, XmssHelpers.AddressTypeHashTree);
                    XmssHelpers.SetTreeHeight(nodeAdrs, (uint)childHeight);
                    XmssHelpers.SetTreeIndex(nodeAdrs, (uint)treeIndex);

                    var concatenated = new byte[2 * attribute.N];
                    Array.Copy(stack[offset - 2], 0, concatenated, 0, attribute.N);
                    Array.Copy(stack[offset - 1], 0, concatenated, attribute.N, attribute.N);

                    var parent = XmssHelpers.ThashH(sha, attribute, concatenated, pubSeed, nodeAdrs, buffer);

                    offset -= 2;
                    stack[offset] = parent;
                    heights[offset] = childHeight + 1;
                    offset++;

                    var node = (1 << (attribute.H - childHeight - 1)) + treeIndex;
                    if (node < storeLimit)
                    {
                        storedNodes[node] = parent;
                    }
                }
            }

            return stack[0];
        }

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
