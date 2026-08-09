using System;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Keys;

namespace NIST.CVP.ACVTS.Libraries.Crypto.XMSS.Native.Keys
{
    public class XmssPublicKey : IXmssPublicKey
    {
        public XmssAttribute XmssAttribute { get; }

        /// <summary>
        /// The n-byte root node of the tree.
        /// </summary>
        public byte[] Root { get; }

        /// <summary>
        /// The n-byte public seed used for hash keys and bitmasks.
        /// </summary>
        public byte[] PubSeed { get; }

        public byte[] Key { get; }

        public XmssPublicKey(XmssAttribute xmssAttribute, byte[] root, byte[] pubSeed)
        {
            XmssAttribute = xmssAttribute;
            Root = root;
            PubSeed = pubSeed;

            Key = new byte[4 + 2 * xmssAttribute.N];
            Array.Copy(xmssAttribute.NumericIdentifier, 0, Key, 0, 4);
            Array.Copy(root, 0, Key, 4, xmssAttribute.N);
            Array.Copy(pubSeed, 0, Key, 4 + xmssAttribute.N, xmssAttribute.N);
        }

        /// <summary>
        /// Construct from the full wire representation, OID || root || SEED.
        /// </summary>
        /// <param name="key">The full wire representation of the public key.</param>
        /// <exception cref="ArgumentException">Thrown when the key cannot be parsed.</exception>
        public XmssPublicKey(byte[] key)
        {
            var mode = AttributesHelper.GetXmssModeFromTypeCode(key.Take(4).ToArray());
            XmssAttribute = AttributesHelper.GetXmssAttribute(mode);

            var expectedKeyLength = 4 + 2 * XmssAttribute.N;
            if (key.Length != expectedKeyLength)
            {
                throw new ArgumentException($"{nameof(key)} was expected to be {expectedKeyLength} bytes, was {key.Length}.");
            }

            Key = key;
            Root = key.Skip(4).Take(XmssAttribute.N).ToArray();
            PubSeed = key.Skip(4 + XmssAttribute.N).Take(XmssAttribute.N).ToArray();
        }
    }
}
