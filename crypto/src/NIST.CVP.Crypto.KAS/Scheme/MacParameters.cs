using System;
using System.Diagnostics.Tracing;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Scheme
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
            
            if (BitString.IsZeroLengthOrNull(ccmNonce))
            {
                throw new ArgumentException(nameof(ccmNonce));
            }

            MacType = macType;
            MacLength = macLength;
            CcmNonce = ccmNonce;
        }
    }
}