using System;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KAS.Scheme
{
    public class MacParameters
    {
        /// <summary>
        /// The type of MAC utilized in the (No)Key Confirmation
        /// </summary>
        public KeyAgreementMacType MacType { get; }
        /// <summary>
        /// The length of the key to plug into a MAC
        /// </summary>
        /// <remarks>This is a new field, and is not (at the time of writing this) being used for old KAS testing.</remarks>
        public int KeyLength { get; }
        /// <summary>
        /// The length of the output mac
        /// </summary>
        public int MacLength { get; }
        /// <summary>
        /// The nonce utilized by a AES-CCM MAC
        /// </summary>
        public BitString CcmNonce { get; }

        public MacParameters(KeyAgreementMacType macType, int keyLength, int macLength)
        {
            if (macType == KeyAgreementMacType.AesCcm)
            {
                throw new ArgumentException(nameof(macType));
            }

            MacType = macType;
            KeyLength = keyLength;
            MacLength = macLength;
        }

        public MacParameters(KeyAgreementMacType macType, int keyLength, int macLength, BitString ccmNonce)
        {
            if (macType != KeyAgreementMacType.AesCcm)
            {
                throw new ArgumentException(nameof(macType));
            }

            MacType = macType;
            KeyLength = keyLength;
            MacLength = macLength;
            CcmNonce = ccmNonce;
        }
    }
}