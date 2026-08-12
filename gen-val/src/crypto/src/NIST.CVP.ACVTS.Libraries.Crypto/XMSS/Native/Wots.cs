using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.XMSS.Native.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.XMSS.Native
{
    public class Wots : IWotsPlus
    {
        private readonly IShaFactory _shaFactory;

        public Wots(IShaFactory shaFactory)
        {
            _shaFactory = shaFactory;
        }

        public byte[] PkGen(XmssAttribute attribute, byte[] skSeed, byte[] pubSeed, uint[] adrs)
        {
            var sha = XmssHelpers.GetSha(_shaFactory, attribute.Mode);
            var buffer = new byte[AttributesHelper.GetBufferByteLengthBasedOnOneWayFunction(attribute.Mode)];

            var chains = ExpandSeed(sha, attribute, skSeed, pubSeed, adrs, buffer);
            for (var i = 0; i < attribute.Len; i++)
            {
                XmssHelpers.SetChainAddress(adrs, (uint)i);
                GenChain(sha, attribute, chains[i], 0, attribute.W - 1, pubSeed, adrs, buffer);
            }

            return Flatten(attribute, chains);
        }

        public byte[] Sign(XmssAttribute attribute, byte[] message, byte[] skSeed, byte[] pubSeed, uint[] adrs)
        {
            var sha = XmssHelpers.GetSha(_shaFactory, attribute.Mode);
            var buffer = new byte[AttributesHelper.GetBufferByteLengthBasedOnOneWayFunction(attribute.Mode)];

            var lengths = XmssHelpers.ChainLengths(attribute, message);

            var chains = ExpandSeed(sha, attribute, skSeed, pubSeed, adrs, buffer);
            for (var i = 0; i < attribute.Len; i++)
            {
                XmssHelpers.SetChainAddress(adrs, (uint)i);
                GenChain(sha, attribute, chains[i], 0, lengths[i], pubSeed, adrs, buffer);
            }

            return Flatten(attribute, chains);
        }

        public byte[] PkFromSig(XmssAttribute attribute, byte[] signature, byte[] message, byte[] pubSeed, uint[] adrs)
        {
            var expectedSignatureLength = attribute.Len * attribute.N;
            if (signature.Length != expectedSignatureLength)
            {
                throw new ArgumentException(
                    $"{nameof(signature)} was expected to be {expectedSignatureLength} bytes, was {signature.Length}.");
            }

            var sha = XmssHelpers.GetSha(_shaFactory, attribute.Mode);
            var buffer = new byte[AttributesHelper.GetBufferByteLengthBasedOnOneWayFunction(attribute.Mode)];

            var lengths = XmssHelpers.ChainLengths(attribute, message);

            var chains = new byte[attribute.Len][];
            for (var i = 0; i < attribute.Len; i++)
            {
                chains[i] = new byte[attribute.N];
                Array.Copy(signature, i * attribute.N, chains[i], 0, attribute.N);

                XmssHelpers.SetChainAddress(adrs, (uint)i);
                GenChain(sha, attribute, chains[i], lengths[i], attribute.W - 1 - lengths[i], pubSeed, adrs, buffer);
            }

            return Flatten(attribute, chains);
        }

        /// <summary>
        /// Expand the n-byte secret seed into the len n-byte chain start values using PRF_keygen.
        /// </summary>
        private static byte[][] ExpandSeed(ISha sha, XmssAttribute attribute, byte[] skSeed, byte[] pubSeed,
            uint[] adrs, byte[] buffer)
        {
            var chains = new byte[attribute.Len][];

            XmssHelpers.SetHashAddress(adrs, 0);
            XmssHelpers.SetKeyAndMask(adrs, 0);
            for (var i = 0; i < attribute.Len; i++)
            {
                XmssHelpers.SetChainAddress(adrs, (uint)i);
                chains[i] = XmssHelpers.PrfKeygen(sha, attribute, skSeed, pubSeed, XmssHelpers.AdrsToBytes(adrs), buffer);
            }

            return chains;
        }

        /// <summary>
        /// Advance an n-byte chain value in place from position <see cref="start"/> by
        /// <see cref="steps"/> iterations of F, capped at w - 1.
        /// </summary>
        private static void GenChain(ISha sha, XmssAttribute attribute, byte[] chain, int start, int steps,
            byte[] pubSeed, uint[] adrs, byte[] buffer)
        {
            for (var i = start; i < start + steps && i < attribute.W; i++)
            {
                XmssHelpers.SetHashAddress(adrs, (uint)i);
                var next = XmssHelpers.ThashF(sha, attribute, chain, pubSeed, adrs, buffer);
                Array.Copy(next, chain, attribute.N);
            }
        }

        private static byte[] Flatten(XmssAttribute attribute, byte[][] chains)
        {
            var result = new byte[attribute.Len * attribute.N];
            for (var i = 0; i < chains.Length; i++)
            {
                Array.Copy(chains[i], 0, result, i * attribute.N, attribute.N);
            }

            return result;
        }
    }
}
