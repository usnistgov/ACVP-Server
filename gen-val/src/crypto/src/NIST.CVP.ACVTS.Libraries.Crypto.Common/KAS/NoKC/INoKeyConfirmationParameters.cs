﻿using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.AES;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.NoKC
{
    /// <summary>
    /// Interface for parameters related to MACing without key confirmation
    /// </summary>
    public interface INoKeyConfirmationParameters
    {
        /// <summary>
        /// The MAC type used in Key Agreement
        /// </summary>
        KeyAgreementMacType KeyAgreementMacType { get; }
        /// <summary>
        /// The MAC length
        /// </summary>
        int MacLength { get; }
        /// <summary>
        /// The Derived Keying Material to be used as the MAC key
        /// </summary>
        BitString DerivedKeyingMaterial { get; }
        /// <summary>
        /// The nonce used (concatenated onto "Standard Test Message")
        /// </summary>
        BitString Nonce { get; }
        /// <summary>
        /// The nonce used for AES-CCM
        /// </summary>
        BitString CcmNonce { get; }
    }
}
