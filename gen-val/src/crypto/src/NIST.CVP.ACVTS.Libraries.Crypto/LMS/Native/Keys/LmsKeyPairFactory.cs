using System;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.LMS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.LMS.Native.Keys
{
    public class LmsKeyPairFactory : ILmsKeyPairFactory
    {
        private readonly ILmOtsKeyPairFactory _lmOtsKeyPairFactory;
        private readonly IShaFactory _shaFactory;

        public LmsKeyPairFactory(ILmOtsKeyPairFactory lmOtsKeyPairFactory, IShaFactory shaFactory)
        {
            _lmOtsKeyPairFactory = lmOtsKeyPairFactory;
            _shaFactory = shaFactory;
        }

        /// <inheritdoc />
        public ILmsKeyPair GetKeyPair(LmsMode lmsMode, LmOtsMode lmOtsMode, byte[] i, byte[] seed, int x = 0)
        {
            var lmsAttribute = AttributesHelper.GetLmsAttribute(lmsMode);
            var lmOtsAttribute = AttributesHelper.GetLmOtsAttribute(lmOtsMode);

            // Perform bound checking for tree
            if (lmsAttribute.M != lmOtsAttribute.N)
            {
                throw new ArgumentException("LMS and LM-OTS attributes have differing output lengths.");
            }
            
            if (x > lmsAttribute.H)
            {
                throw new ArgumentException("X cannot exceed the height of the tree");
            }
            
            // Perform bounds checking for private key
            if (i?.Length != 16)
            {
                throw new ArgumentOutOfRangeException($"{nameof(i)} was expected to be 16 bytes, was {i?.Length}.");
            }

            if (seed?.Length < lmOtsAttribute.N)
            {
                throw new ArgumentException($"Expected byte array of at least length {lmOtsAttribute.N}, got {seed?.Length}", nameof(seed));
            }
            
            if (lmsAttribute.M != lmOtsAttribute.N)
            {
                throw new ArgumentException($"lms.M and lmOts.N must be equal, got {lmsAttribute.M} and {lmOtsAttribute.N}");
            }
            
            // Generate keys
            return GenerateKeyPair(lmsAttribute, lmOtsAttribute, i, seed, x);
        }

        private LmsKeyPair GenerateKeyPair(LmsAttribute lmsAttribute, LmOtsAttribute lmOtsAttribute, byte[] I, byte[] seed, int x)
        {
            var sha = LmsHelpers.GetSha(_shaFactory, lmsAttribute.Mode);
            var numLmOtsKeys = 1 << lmsAttribute.H;
            var stack = new Stack<byte[]>();

            var iBits = I.BitLength();
            var mBits = lmsAttribute.M * 8;
            
            var buffer = new byte[AttributesHelper.GetBufferByteLengthBasedOnOneWayFunction(lmsAttribute.Mode)];
            var bufferBits = AttributesHelper.GetBufferByteLengthBasedOnOneWayFunction(lmsAttribute.Mode) * 8;

            var storedTreeLength = x <= 0 ? 0 : (1 << (x + 1));
            var storedTree = new byte[storedTreeLength][];
            
            // storedTree is 1-indexed so storedTree[0], or T[0], is not used. We probably need to allocate it anyways so we can safely write to JSON later on for Pools
            if (storedTreeLength != 0)
            {
                storedTree[0] = Array.Empty<byte>();
            }
            
            int r, j;
            
            for (var i = 0; i < numLmOtsKeys; i++)
            {
                r = i + numLmOtsKeys;
                var temp = new byte[lmsAttribute.M];
                
                // temp = H(I || u32str(r) || u16str(D_LEAF) || OTS_PUB_HASH[i])
                sha.Init();
                sha.Update(I, iBits);
                sha.Update(r, 32);
                sha.Update(LmsHelpers.D_LEAF, 16);
                
                var lmOtsPublicKey = _lmOtsKeyPairFactory.GetKeyPair(lmOtsAttribute.Mode, I, i.GetBytes(), seed).PublicKey.K;

                sha.Update(lmOtsPublicKey, mBits);
                sha.Final(buffer, bufferBits);
                
                Array.Copy(buffer, temp, temp.Length);

                // If this is a tree node we need to store
                if (r < storedTreeLength)
                {
                    storedTree[r] = new byte[temp.Length];
                    Array.Copy(temp, storedTree[r], temp.Length);
                }
                
                j = i;
                while (j % 2 == 1)
                {
                    r = (r - 1) / 2;
                    j = (j - 1) / 2;
                    
                    // temp = H(I || u32str(r) || u16str(D_INTR) || left_side (stack) || temp)
                    sha.Init();
                    sha.Update(I, iBits);
                    sha.Update(r, 32);
                    sha.Update(LmsHelpers.D_INTR, 16);
                    sha.Update(stack.Pop(), mBits);
                    sha.Update(temp, mBits);
                    sha.Final(buffer, bufferBits);
                    
                    Array.Copy(buffer, temp, temp.Length);
                    
                    // If this is a tree node we need to store
                    if (r < storedTreeLength)
                    {
                        storedTree[r] = new byte[temp.Length];
                        Array.Copy(temp, storedTree[r], temp.Length);
                    }
                }

                stack.Push(temp);
            }

            var publicKey = stack.Pop();
            
            // u32str(type) || u32str(otstype) || I || T[1]
            var key = new byte[4 + 4 + 16 + publicKey.Length];
            Array.Copy(lmsAttribute.NumericIdentifier, 0, key, 0, 4);
            Array.Copy(lmOtsAttribute.NumericIdentifier, 0, key, 4, 4);
            Array.Copy(I, 0, key, 4 + 4, 16);
            Array.Copy(publicKey, 0, key, 4 + 4 + 16, publicKey.Length);

            return new LmsKeyPair
            {
                LmsAttribute = lmsAttribute,
                LmOtsAttribute = lmOtsAttribute,
                PrivateKey = new LmsPrivateKey(lmsAttribute, lmOtsAttribute, I, seed, 0, x, storedTree),
                PublicKey = new LmsPublicKey(lmsAttribute, key)
            };
        }
    }
}
