using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.XMSS.Native.Helpers
{
    /// <summary>
    /// Static algorithm helpers for XMSS per https://datatracker.ietf.org/doc/html/rfc8391 and
    /// https://nvlpubs.nist.gov/nistpubs/SpecialPublications/NIST.SP.800-208.pdf.
    ///
    /// The keyed hash functions F, H, H_msg and PRF are domain separated with a prefix
    /// toByte(X, paddingLength); the hash address (ADRS) scheme provides the per-call
    /// uniqueness of keys and bitmasks.
    /// </summary>
    public static class XmssHelpers
    {
        public const int HashPaddingF = 0;
        public const int HashPaddingH = 1;
        public const int HashPaddingHashMessage = 2;
        public const int HashPaddingPrf = 3;
        public const int HashPaddingPrfKeygen = 4;

        public const uint AddressTypeOts = 0;
        public const uint AddressTypeLtree = 1;
        public const uint AddressTypeHashTree = 2;

        /* The eight word hash address layout of RFC 8391 section 2.5:
           word 0 layer, words 1-2 tree, word 3 type,
           word 4 OTS/L-tree address, word 5 chain/tree height,
           word 6 hash/tree index, word 7 keyAndMask. */

        public static void SetLayerAddress(uint[] adrs, uint layer)
        {
            adrs[0] = layer;
        }

        public static void SetTreeAddress(uint[] adrs, ulong tree)
        {
            adrs[1] = (uint)(tree >> 32);
            adrs[2] = (uint)tree;
        }

        public static void SetType(uint[] adrs, uint type)
        {
            adrs[3] = type;
        }

        public static void SetOtsAddress(uint[] adrs, uint ots)
        {
            adrs[4] = ots;
        }

        public static void SetLtreeAddress(uint[] adrs, uint ltree)
        {
            adrs[4] = ltree;
        }

        public static void SetChainAddress(uint[] adrs, uint chain)
        {
            adrs[5] = chain;
        }

        public static void SetTreeHeight(uint[] adrs, uint treeHeight)
        {
            adrs[5] = treeHeight;
        }

        public static void SetHashAddress(uint[] adrs, uint hash)
        {
            adrs[6] = hash;
        }

        public static void SetTreeIndex(uint[] adrs, uint treeIndex)
        {
            adrs[6] = treeIndex;
        }

        public static void SetKeyAndMask(uint[] adrs, uint keyAndMask)
        {
            adrs[7] = keyAndMask;
        }

        /// <summary>
        /// Serialize an eight word hash address to its 32 byte big-endian representation.
        /// </summary>
        /// <param name="adrs">The eight word hash address.</param>
        /// <returns>The 32 byte representation.</returns>
        public static byte[] AdrsToBytes(uint[] adrs)
        {
            var bytes = new byte[32];
            for (var i = 0; i < 8; i++)
            {
                bytes[i * 4] = (byte)(adrs[i] >> 24);
                bytes[i * 4 + 1] = (byte)(adrs[i] >> 16);
                bytes[i * 4 + 2] = (byte)(adrs[i] >> 8);
                bytes[i * 4 + 3] = (byte)adrs[i];
            }

            return bytes;
        }

        /// <summary>
        /// toByte(x, y) from https://datatracker.ietf.org/doc/html/rfc8391#section-2.4 -
        /// the y-byte big-endian representation of x.
        /// </summary>
        /// <param name="value">The non-negative value to represent.</param>
        /// <param name="length">The byte length of the representation.</param>
        /// <returns>The big-endian representation.</returns>
        public static byte[] ToByte(long value, int length)
        {
            var bytes = new byte[length];
            for (var i = length - 1; i >= 0 && value != 0; i--)
            {
                bytes[i] = (byte)value;
                value >>= 8;
            }

            return bytes;
        }

        /// <summary>
        /// PRF: H(toByte(3, paddingLength) || KEY || M) over a 32-byte input M.
        ///
        /// https://datatracker.ietf.org/doc/html/rfc8391#section-5.1
        /// </summary>
        /// <param name="sha">The sha instance to use.</param>
        /// <param name="attribute">The XMSS parameter set attributes in use.</param>
        /// <param name="key">The n-byte key.</param>
        /// <param name="input32">The 32-byte input, a serialized hash address or index.</param>
        /// <param name="buffer">The buffer that SHA finalizes upon.</param>
        /// <returns>The n-byte PRF output.</returns>
        public static byte[] Prf(ISha sha, XmssAttribute attribute, byte[] key, byte[] input32, byte[] buffer)
        {
            sha.Init();
            sha.Update(ToByte(HashPaddingPrf, attribute.PaddingLength), attribute.PaddingLength * 8);
            sha.Update(key, key.BitLength());
            sha.Update(input32, input32.BitLength());
            sha.Final(buffer, buffer.BitLength());

            var result = new byte[attribute.N];
            Array.Copy(buffer, result, result.Length);
            return result;
        }

        /// <summary>
        /// PRF_keygen: H(toByte(4, paddingLength) || S_XMSS || SEED || ADRS), deriving a WOTS+
        /// chain secret from the private key's secret seed.
        ///
        /// https://nvlpubs.nist.gov/nistpubs/SpecialPublications/NIST.SP.800-208.pdf section 5
        /// </summary>
        /// <param name="sha">The sha instance to use.</param>
        /// <param name="attribute">The XMSS parameter set attributes in use.</param>
        /// <param name="skSeed">The n-byte secret seed.</param>
        /// <param name="pubSeed">The n-byte public seed.</param>
        /// <param name="adrsBytes">The 32-byte serialized hash address.</param>
        /// <param name="buffer">The buffer that SHA finalizes upon.</param>
        /// <returns>The n-byte chain secret.</returns>
        public static byte[] PrfKeygen(ISha sha, XmssAttribute attribute, byte[] skSeed, byte[] pubSeed,
            byte[] adrsBytes, byte[] buffer)
        {
            sha.Init();
            sha.Update(ToByte(HashPaddingPrfKeygen, attribute.PaddingLength), attribute.PaddingLength * 8);
            sha.Update(skSeed, skSeed.BitLength());
            sha.Update(pubSeed, pubSeed.BitLength());
            sha.Update(adrsBytes, adrsBytes.BitLength());
            sha.Final(buffer, buffer.BitLength());

            var result = new byte[attribute.N];
            Array.Copy(buffer, result, result.Length);
            return result;
        }

        /// <summary>
        /// The keyed hash function F over an n-byte input:
        /// F(KEY, M) = H(toByte(0, paddingLength) || KEY || M xor bitmask), with the key and
        /// bitmask derived from the public seed and hash address.
        ///
        /// https://datatracker.ietf.org/doc/html/rfc8391#section-5.1
        /// </summary>
        /// <param name="sha">The sha instance to use.</param>
        /// <param name="attribute">The XMSS parameter set attributes in use.</param>
        /// <param name="input">The n-byte input.</param>
        /// <param name="pubSeed">The n-byte public seed.</param>
        /// <param name="adrs">The eight word hash address; keyAndMask is mutated during computation.</param>
        /// <param name="buffer">The buffer that SHA finalizes upon.</param>
        /// <returns>The n-byte output.</returns>
        public static byte[] ThashF(ISha sha, XmssAttribute attribute, byte[] input, byte[] pubSeed, uint[] adrs,
            byte[] buffer)
        {
            SetKeyAndMask(adrs, 0);
            var key = Prf(sha, attribute, pubSeed, AdrsToBytes(adrs), buffer);

            SetKeyAndMask(adrs, 1);
            var bitmask = Prf(sha, attribute, pubSeed, AdrsToBytes(adrs), buffer);

            var masked = new byte[attribute.N];
            for (var i = 0; i < attribute.N; i++)
            {
                masked[i] = (byte)(input[i] ^ bitmask[i]);
            }

            sha.Init();
            sha.Update(ToByte(HashPaddingF, attribute.PaddingLength), attribute.PaddingLength * 8);
            sha.Update(key, key.BitLength());
            sha.Update(masked, masked.BitLength());
            sha.Final(buffer, buffer.BitLength());

            var result = new byte[attribute.N];
            Array.Copy(buffer, result, result.Length);
            return result;
        }

        /// <summary>
        /// The keyed hash function H over a 2n-byte input (two tree nodes):
        /// H(KEY, M) = H(toByte(1, paddingLength) || KEY || M xor bitmask), with the key and
        /// 2n-byte bitmask derived from the public seed and hash address.
        ///
        /// https://datatracker.ietf.org/doc/html/rfc8391#section-5.1
        /// </summary>
        /// <param name="sha">The sha instance to use.</param>
        /// <param name="attribute">The XMSS parameter set attributes in use.</param>
        /// <param name="input">The 2n-byte input, the left node followed by the right node.</param>
        /// <param name="pubSeed">The n-byte public seed.</param>
        /// <param name="adrs">The eight word hash address; keyAndMask is mutated during computation.</param>
        /// <param name="buffer">The buffer that SHA finalizes upon.</param>
        /// <returns>The n-byte output.</returns>
        public static byte[] ThashH(ISha sha, XmssAttribute attribute, byte[] input, byte[] pubSeed, uint[] adrs,
            byte[] buffer)
        {
            SetKeyAndMask(adrs, 0);
            var key = Prf(sha, attribute, pubSeed, AdrsToBytes(adrs), buffer);

            SetKeyAndMask(adrs, 1);
            var bitmaskLeft = Prf(sha, attribute, pubSeed, AdrsToBytes(adrs), buffer);

            SetKeyAndMask(adrs, 2);
            var bitmaskRight = Prf(sha, attribute, pubSeed, AdrsToBytes(adrs), buffer);

            var masked = new byte[2 * attribute.N];
            for (var i = 0; i < attribute.N; i++)
            {
                masked[i] = (byte)(input[i] ^ bitmaskLeft[i]);
                masked[attribute.N + i] = (byte)(input[attribute.N + i] ^ bitmaskRight[i]);
            }

            sha.Init();
            sha.Update(ToByte(HashPaddingH, attribute.PaddingLength), attribute.PaddingLength * 8);
            sha.Update(key, key.BitLength());
            sha.Update(masked, masked.BitLength());
            sha.Final(buffer, buffer.BitLength());

            var result = new byte[attribute.N];
            Array.Copy(buffer, result, result.Length);
            return result;
        }

        /// <summary>
        /// H_msg: H(toByte(2, paddingLength) || r || root || toByte(idx, n) || M), the
        /// randomized message digest that WOTS+ signs.
        ///
        /// https://datatracker.ietf.org/doc/html/rfc8391#section-5.1
        /// </summary>
        /// <param name="sha">The sha instance to use.</param>
        /// <param name="attribute">The XMSS parameter set attributes in use.</param>
        /// <param name="r">The n-byte message randomizer from the signature.</param>
        /// <param name="root">The n-byte root node of the tree.</param>
        /// <param name="idx">The leaf index the message is signed with.</param>
        /// <param name="message">The message.</param>
        /// <param name="buffer">The buffer that SHA finalizes upon.</param>
        /// <returns>The n-byte message digest.</returns>
        public static byte[] HashMessage(ISha sha, XmssAttribute attribute, byte[] r, byte[] root, long idx,
            byte[] message, byte[] buffer)
        {
            sha.Init();
            sha.Update(ToByte(HashPaddingHashMessage, attribute.PaddingLength), attribute.PaddingLength * 8);
            sha.Update(r, r.BitLength());
            sha.Update(root, root.BitLength());
            sha.Update(ToByte(idx, attribute.N), attribute.N * 8);
            sha.Update(message, message.BitLength());
            sha.Final(buffer, buffer.BitLength());

            var result = new byte[attribute.N];
            Array.Copy(buffer, result, result.Length);
            return result;
        }

        /// <summary>
        /// base_w from https://datatracker.ietf.org/doc/html/rfc8391#section-2.6 -
        /// interprets an array of bytes as <see cref="outputLength"/> integers in base w.
        /// </summary>
        /// <param name="attribute">The XMSS parameter set attributes in use.</param>
        /// <param name="input">The bytes to interpret.</param>
        /// <param name="outputLength">The number of base w digits to produce.</param>
        /// <returns>The base w digits.</returns>
        public static int[] BaseW(XmssAttribute attribute, byte[] input, int outputLength)
        {
            var logW = FloorLog2(attribute.W);
            var output = new int[outputLength];

            var inIndex = 0;
            var total = 0;
            var bits = 0;
            for (var consumed = 0; consumed < outputLength; consumed++)
            {
                if (bits == 0)
                {
                    total = input[inIndex];
                    inIndex++;
                    bits += 8;
                }

                bits -= logW;
                output[consumed] = (total >> bits) & (attribute.W - 1);
            }

            return output;
        }

        /// <summary>
        /// Derive the WOTS+ chain lengths for an n-byte message digest: the len1 base w digits
        /// of the digest followed by the len2 base w digits of their checksum.
        ///
        /// https://datatracker.ietf.org/doc/html/rfc8391#section-3.1.5
        /// </summary>
        /// <param name="attribute">The XMSS parameter set attributes in use.</param>
        /// <param name="message">The n-byte message digest.</param>
        /// <returns>The len chain lengths.</returns>
        public static int[] ChainLengths(XmssAttribute attribute, byte[] message)
        {
            var logW = FloorLog2(attribute.W);
            var lengths = new int[attribute.Len];

            var messageBaseW = BaseW(attribute, message, attribute.Len1);
            Array.Copy(messageBaseW, lengths, attribute.Len1);

            var csum = 0;
            for (var i = 0; i < attribute.Len1; i++)
            {
                csum += attribute.W - 1 - messageBaseW[i];
            }

            /* Make sure expected empty zero bits are the least significant bits. */
            csum <<= 8 - ((attribute.Len2 * logW) % 8);
            var csumBytes = ToByte(csum, (attribute.Len2 * logW + 7) / 8);

            var csumBaseW = BaseW(attribute, csumBytes, attribute.Len2);
            Array.Copy(csumBaseW, 0, lengths, attribute.Len1, attribute.Len2);

            return lengths;
        }

        /// <summary>
        /// Compress a WOTS+ public key of len chains into a single n-byte leaf with the
        /// unbalanced binary L-tree of https://datatracker.ietf.org/doc/html/rfc8391#section-4.1.5.
        /// The provided <see cref="wotsPk"/> array is consumed by the computation.
        /// </summary>
        public static byte[] LTree(ISha sha, XmssAttribute attribute, byte[][] wotsPk, byte[] pubSeed, uint[] adrs,
            byte[] buffer)
        {
            var l = attribute.Len;
            uint height = 0;

            SetTreeHeight(adrs, height);

            while (l > 1)
            {
                var parentNodes = l >> 1;
                for (var i = 0; i < parentNodes; i++)
                {
                    SetTreeIndex(adrs, (uint)i);

                    var concatenated = new byte[2 * attribute.N];
                    Array.Copy(wotsPk[2 * i], 0, concatenated, 0, attribute.N);
                    Array.Copy(wotsPk[2 * i + 1], 0, concatenated, attribute.N, attribute.N);

                    wotsPk[i] = ThashH(sha, attribute, concatenated, pubSeed, adrs, buffer);
                }

                /* If the row contained an odd number of nodes, the last node was not hashed;
                   it is pulled up to the next layer instead. */
                if ((l & 1) == 1)
                {
                    wotsPk[l >> 1] = wotsPk[l - 1];
                    l = (l >> 1) + 1;
                }
                else
                {
                    l >>= 1;
                }

                height++;
                SetTreeHeight(adrs, height);
            }

            return wotsPk[0];
        }

        /// <summary>
        /// Compute the root implied by a leaf and its authentication path per
        /// https://datatracker.ietf.org/doc/html/rfc8391#section-4.1.10 Algorithm 13.
        /// </summary>
        public static byte[] ComputeRoot(ISha sha, XmssAttribute attribute, byte[] leaf, uint leafIndex,
            byte[][] auth, byte[] pubSeed, uint[] adrs, byte[] buffer)
        {
            var nodes = new byte[2 * attribute.N];

            /* If the leaf index is odd, the current node is a right child and the
               authentication path node goes on the left; otherwise the other way around. */
            if ((leafIndex & 1) == 1)
            {
                Array.Copy(auth[0], 0, nodes, 0, attribute.N);
                Array.Copy(leaf, 0, nodes, attribute.N, attribute.N);
            }
            else
            {
                Array.Copy(leaf, 0, nodes, 0, attribute.N);
                Array.Copy(auth[0], 0, nodes, attribute.N, attribute.N);
            }

            for (var i = 0; i < attribute.H - 1; i++)
            {
                SetTreeHeight(adrs, (uint)i);
                leafIndex >>= 1;
                SetTreeIndex(adrs, leafIndex);

                var parent = ThashH(sha, attribute, nodes, pubSeed, adrs, buffer);
                if ((leafIndex & 1) == 1)
                {
                    Array.Copy(auth[i + 1], 0, nodes, 0, attribute.N);
                    Array.Copy(parent, 0, nodes, attribute.N, attribute.N);
                }
                else
                {
                    Array.Copy(parent, 0, nodes, 0, attribute.N);
                    Array.Copy(auth[i + 1], 0, nodes, attribute.N, attribute.N);
                }
            }

            SetTreeHeight(adrs, (uint)(attribute.H - 1));
            leafIndex >>= 1;
            SetTreeIndex(adrs, leafIndex);

            return ThashH(sha, attribute, nodes, pubSeed, adrs, buffer);
        }

        /// <summary>
        /// Split a flat len * n byte WOTS+ key or signature into its len n-byte chains.
        /// </summary>
        public static byte[][] Unflatten(XmssAttribute attribute, byte[] flat)
        {
            var chains = new byte[attribute.Len][];
            for (var i = 0; i < attribute.Len; i++)
            {
                chains[i] = new byte[attribute.N];
                Array.Copy(flat, i * attribute.N, chains[i], 0, attribute.N);
            }

            return chains;
        }

        /// <summary>
        /// Gets the appropriate SHA instance based on the <see cref="XmssMode"/>.
        /// </summary>
        /// <param name="shaFactory">The <see cref="IShaFactory"/> to retrieve an instance from.</param>
        /// <param name="mode">The <see cref="XmssMode"/> in use.</param>
        /// <returns>An instance of <see cref="ISha"/></returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the <see cref="XmssMode"/> is invalid.</exception>
        public static ISha GetSha(IShaFactory shaFactory, XmssMode mode)
        {
            switch (mode)
            {
                case XmssMode.XMSS_SHA2_10_256:
                case XmssMode.XMSS_SHA2_16_256:
                case XmssMode.XMSS_SHA2_20_256:
                case XmssMode.XMSS_SHA2_10_192:
                case XmssMode.XMSS_SHA2_16_192:
                case XmssMode.XMSS_SHA2_20_192:
                    return shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d256));
                case XmssMode.XMSS_SHAKE256_10_256:
                case XmssMode.XMSS_SHAKE256_16_256:
                case XmssMode.XMSS_SHAKE256_20_256:
                case XmssMode.XMSS_SHAKE256_10_192:
                case XmssMode.XMSS_SHAKE256_16_192:
                case XmssMode.XMSS_SHAKE256_20_192:
                    return shaFactory.GetShaInstance(new HashFunction(ModeValues.SHAKE, DigestSizes.d256));
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), $"Unsupported {nameof(mode)} for retrieving a hash function.");
            }
        }

        /// <summary>
        /// The floor of the base 2 logarithm of a positive value.
        /// </summary>
        public static int FloorLog2(int value)
        {
            var result = 0;
            while (value > 1)
            {
                value >>= 1;
                result++;
            }

            return result;
        }
    }
}
