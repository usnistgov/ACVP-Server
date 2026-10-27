using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Helpers
{
    public static class AttributesHelper
    {
        public static readonly Dictionary<XmssMode, XmssAttribute> XmssAttributes =
            new()
            {
                {
                    XmssMode.XMSS_SHA2_10_256,
                    new XmssAttribute(
                    XmssMode.XMSS_SHA2_10_256, 0x00000001.GetBytes(), 32, 10, 16, 64, 3, 32, ModeValues.SHA2)
                },
                {
                    XmssMode.XMSS_SHA2_16_256,
                    new XmssAttribute(
                    XmssMode.XMSS_SHA2_16_256, 0x00000002.GetBytes(), 32, 16, 16, 64, 3, 32, ModeValues.SHA2)
                },
                {
                    XmssMode.XMSS_SHA2_20_256,
                    new XmssAttribute(
                    XmssMode.XMSS_SHA2_20_256, 0x00000003.GetBytes(), 32, 20, 16, 64, 3, 32, ModeValues.SHA2)
                },

                {
                    XmssMode.XMSS_SHA2_10_192,
                    new XmssAttribute(
                    XmssMode.XMSS_SHA2_10_192, 0x0000000D.GetBytes(), 24, 10, 16, 48, 3, 4, ModeValues.SHA2)
                },
                {
                    XmssMode.XMSS_SHA2_16_192,
                    new XmssAttribute(
                    XmssMode.XMSS_SHA2_16_192, 0x0000000E.GetBytes(), 24, 16, 16, 48, 3, 4, ModeValues.SHA2)
                },
                {
                    XmssMode.XMSS_SHA2_20_192,
                    new XmssAttribute(
                    XmssMode.XMSS_SHA2_20_192, 0x0000000F.GetBytes(), 24, 20, 16, 48, 3, 4, ModeValues.SHA2)
                },

                {
                    XmssMode.XMSS_SHAKE256_10_256,
                    new XmssAttribute(
                    XmssMode.XMSS_SHAKE256_10_256, 0x00000010.GetBytes(), 32, 10, 16, 64, 3, 32, ModeValues.SHAKE)
                },
                {
                    XmssMode.XMSS_SHAKE256_16_256,
                    new XmssAttribute(
                    XmssMode.XMSS_SHAKE256_16_256, 0x00000011.GetBytes(), 32, 16, 16, 64, 3, 32, ModeValues.SHAKE)
                },
                {
                    XmssMode.XMSS_SHAKE256_20_256,
                    new XmssAttribute(
                    XmssMode.XMSS_SHAKE256_20_256, 0x00000012.GetBytes(), 32, 20, 16, 64, 3, 32, ModeValues.SHAKE)
                },

                {
                    XmssMode.XMSS_SHAKE256_10_192,
                    new XmssAttribute(
                    XmssMode.XMSS_SHAKE256_10_192, 0x00000013.GetBytes(), 24, 10, 16, 48, 3, 4, ModeValues.SHAKE)
                },
                {
                    XmssMode.XMSS_SHAKE256_16_192,
                    new XmssAttribute(
                    XmssMode.XMSS_SHAKE256_16_192, 0x00000014.GetBytes(), 24, 16, 16, 48, 3, 4, ModeValues.SHAKE)
                },
                {
                    XmssMode.XMSS_SHAKE256_20_192,
                    new XmssAttribute(
                    XmssMode.XMSS_SHAKE256_20_192, 0x00000015.GetBytes(), 24, 20, 16, 48, 3, 4, ModeValues.SHAKE)
                },
            };

        /// <summary>
        /// Get the <see cref="XmssAttribute"/> associated to the provided <see cref="XmssMode"/>.
        /// </summary>
        /// <param name="xmssMode">The mode to get attributes for.</param>
        /// <returns>The attributes associated to the mode.</returns>
        /// <exception cref="ArgumentException">Thrown when the mode cannot be mapped.</exception>
        public static XmssAttribute GetXmssAttribute(XmssMode xmssMode)
        {
            if (!XmssAttributes.TryFirst(w => w.Key == xmssMode, out var result))
            {
                throw new ArgumentException($"Couldn't map {nameof(xmssMode)} for retrieving attributes.");
            }

            return result.Value;
        }

        /// <summary>
        /// Gets the length in bytes needed for the buffer used in an underlying one way function.
        ///
        /// The 192-bit parameter sets use only the 24 most significant bytes from the 32 bytes
        /// available from the output of the one way function, but the buffer needs to be written
        /// to in its entirety.
        /// </summary>
        /// <param name="mode">The <see cref="XmssMode"/> used for determining the output buffer size.</param>
        /// <returns>The length in bytes of the buffer used for the one way function.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when an invalid mode is provided.</exception>
        public static int GetBufferByteLengthBasedOnOneWayFunction(XmssMode mode)
        {
            switch (mode)
            {
                case XmssMode.XMSS_SHA2_10_256:
                case XmssMode.XMSS_SHA2_16_256:
                case XmssMode.XMSS_SHA2_20_256:
                case XmssMode.XMSS_SHA2_10_192:
                case XmssMode.XMSS_SHA2_16_192:
                case XmssMode.XMSS_SHA2_20_192:
                case XmssMode.XMSS_SHAKE256_10_256:
                case XmssMode.XMSS_SHAKE256_16_256:
                case XmssMode.XMSS_SHAKE256_20_256:
                case XmssMode.XMSS_SHAKE256_10_192:
                case XmssMode.XMSS_SHAKE256_16_192:
                case XmssMode.XMSS_SHAKE256_20_192:
                    return 32;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), $"Unsupported {nameof(mode)} for retrieving a buffer length.");
            }
        }

        /// <summary>
        /// Get the <see cref="XmssMode"/> from the provided four byte type code.
        /// </summary>
        /// <param name="typeCode">The four byte parameter set identifier.</param>
        /// <returns>The mode when found, otherwise <see cref="XmssMode.Invalid"/>.</returns>
        public static XmssMode GetXmssModeFromTypeCode(byte[] typeCode)
        {
            if (typeCode == null)
            {
                return XmssMode.Invalid;
            }

            return XmssAttributes
                .FirstOrDefault(w => w.Value.NumericIdentifier.SequenceEqual(typeCode))
                .Key;
        }
    }
}
