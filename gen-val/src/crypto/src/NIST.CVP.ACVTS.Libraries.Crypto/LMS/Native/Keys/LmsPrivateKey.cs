using System;
using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys;

namespace NIST.CVP.ACVTS.Libraries.Crypto.LMS.Native.Keys
{
    public class LmsPrivateKey : ILmsPrivateKey
    {
        private readonly object _lockObject = new();
        private readonly int _maxQ;
        private int _q;

        /// <summary>
        /// The number of layers that are precomputed for this tree, X=1 means only the public key (root) is stored, X=H means the full tree is stored
        /// </summary>
        public int X { get; }
        public byte[][] T { get; }

        public LmsAttribute LmsAttribute { get; }
        public LmOtsAttribute LmOtsAttribute { get; }
        public byte[] I { get; }
        public byte[] Seed { get; }
        public bool IsExhausted => _q > _maxQ;
        
        /// <summary>
        /// LMS Private Key Constructor
        /// </summary>
        /// <param name="lmsAttribute"></param>
        /// <param name="lmOtsAttribute"></param>
        /// <param name="i"></param>
        /// <param name="seed"></param>
        /// <param name="q"></param>
        /// <param name="x"></param>
        /// <param name="hashes">The list of precomputed hash values that make the tree. Index 1 is the root, index 2-3 are the first layer, etc. Must be 2^X in size.</param>
        /// <exception cref="ArgumentException"></exception>
        public LmsPrivateKey(LmsAttribute lmsAttribute, LmOtsAttribute lmOtsAttribute, byte[] i, byte[] seed, int q, int x, byte[][] hashes)
        {
            LmsAttribute = lmsAttribute;
            LmOtsAttribute = lmOtsAttribute;
            I = i;
            Seed = seed;
            _q = q;
            _maxQ = (int)BigInteger.Pow(2, lmsAttribute.H) - 1;
            X = x;
            
            if (X > lmsAttribute.H)
            {
                throw new ArgumentException("X cannot exceed the height of the tree");
            }
            
            if (x > 0)
            {
                if (hashes.Length == (2 << X) - 1)
                {
                    // Just tree values, hashes[0] is T[1]
                    T = new byte[hashes.Length+1][];
                    for (var j = 1; j < hashes.Length; j++)
                    {
                        T[j] = hashes[j-1];
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

        public void SetQ(int q)
        {
            if (q > _maxQ)
                throw new ArgumentOutOfRangeException(nameof(q), $"Cannot exceed max of {_maxQ}");

            if (q < 0)
                throw new ArgumentOutOfRangeException(nameof(q), "Cannot be negative.");

            lock (_lockObject)
                _q = q;
        }

        public int? GetQ(bool withIncrement)
        {
            lock (_lockObject)
            {
                var q = _q;

                if (IsExhausted)
                    return null;

                if (withIncrement)
                {
                    _q++;
                }

                return q;
            }
        }
    }
}
