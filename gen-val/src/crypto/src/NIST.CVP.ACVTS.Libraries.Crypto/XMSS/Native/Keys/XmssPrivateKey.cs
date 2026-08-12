using System;
using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Keys;

namespace NIST.CVP.ACVTS.Libraries.Crypto.XMSS.Native.Keys
{
    public class XmssPrivateKey : IXmssPrivateKey
    {
        private readonly object _lockObject = new();
        private readonly int _maxIdx;
        private int _idx;

        /// <summary>
        /// The number of layers that are precomputed for this tree, X=1 means only the root and its
        /// children are stored, X=H means the full tree is stored
        /// </summary>
        public int X { get; }
        public byte[][] T { get; }

        public XmssAttribute XmssAttribute { get; }
        public byte[] SkSeed { get; }
        public byte[] SkPrf { get; }
        public byte[] PubSeed { get; }
        public byte[] Root { get; }
        public bool IsExhausted => _idx > _maxIdx;

        /// <summary>
        /// XMSS Private Key Constructor
        /// </summary>
        /// <param name="xmssAttribute"></param>
        /// <param name="skSeed"></param>
        /// <param name="skPrf"></param>
        /// <param name="pubSeed"></param>
        /// <param name="root"></param>
        /// <param name="idx"></param>
        /// <param name="x"></param>
        /// <param name="hashes">The list of precomputed hash values that make the tree. Index 1 is the root, index 2-3 are the first layer, etc. Must be 2^X in size.</param>
        /// <exception cref="ArgumentException"></exception>
        public XmssPrivateKey(XmssAttribute xmssAttribute, byte[] skSeed, byte[] skPrf, byte[] pubSeed, byte[] root,
            int idx, int x, byte[][] hashes)
        {
            XmssAttribute = xmssAttribute;
            SkSeed = skSeed;
            SkPrf = skPrf;
            PubSeed = pubSeed;
            Root = root;
            _idx = idx;
            _maxIdx = (int)BigInteger.Pow(2, xmssAttribute.H) - 1;
            X = x;

            if (X > xmssAttribute.H)
            {
                throw new ArgumentException("X cannot exceed the height of the tree");
            }

            if (x > 0)
            {
                if (hashes.Length == (2 << X) - 1)
                {
                    // Just tree values, hashes[0] is T[1]
                    T = new byte[hashes.Length + 1][];
                    for (var j = 1; j <= hashes.Length; j++)
                    {
                        T[j] = hashes[j - 1];
                    }
                }
                else if (hashes.Length == (2 << X))
                {
                    // Root of the tree is hashes[1], not hashes[0]. T[0] will be left empty, uninitialized to discourage usage
                    T = new byte[hashes.Length][];
                    for (var j = 1; j < hashes.Length; j++)
                    {
                        T[j] = hashes[j];
                    }
                }
                else
                {
                    throw new ArgumentException($"Incomplete top X layers of tree provided. X = {x}, hash count = {hashes.Length}");
                }
            }
        }

        public byte[] GetTreeNodeAtIndex(int i)
        {
            return T[i];
        }

        public bool HasPrecomputedHash(int r)
        {
            if (T == null)
            {
                return false;
            }

            if (r >= T.Length)
            {
                return false;
            }

            return T[r] != null;
        }

        public void SetIdx(int idx)
        {
            if (idx > _maxIdx)
                throw new ArgumentOutOfRangeException(nameof(idx), $"Cannot exceed max of {_maxIdx}");

            if (idx < 0)
                throw new ArgumentOutOfRangeException(nameof(idx), "Cannot be negative.");

            lock (_lockObject)
                _idx = idx;
        }

        public int? GetIdx(bool withIncrement = true)
        {
            lock (_lockObject)
            {
                var idx = _idx;

                if (IsExhausted)
                    return null;

                if (withIncrement)
                {
                    _idx++;
                }

                return idx;
            }
        }
    }
}
