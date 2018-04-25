using System;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KAS.Schema
{
    public class MacParameters
    {
        /// <summary>
        /// The type of MAC utilized in the (No)Key Confirmation
        /// </summary>
        public KeyAgreementMacType MacType { get; }
        /// <summary>
        /// The length of the output mac
        /// </summary>
        public int MacLength { get; }
        /// <summary>
        /// The nonce utilized by a AES-CCM MAC
        /// </summary>
        public BitString CcmNonce { get; }

        public MacParameters(KeyAgreementMacType macType, int macLength)
        {
            if (macType == KeyAgreementMacType.AesCcm)
            {
                throw new ArgumentException(nameof(macType));
            }

            MacType = macType;
            MacLength = macLength;
        }

        public MacParameters(KeyAgreementMacType macType, int macLength, BitString ccmNonce)
        {
            if (macType != KeyAgreementMacType.AesCcm)
            {
                throw new ArgumentException(nameof(macType));
            }

            MacType = macType;
            MacLength = macLength;
            CcmNonce = ccmNonce;
        }
    }
}