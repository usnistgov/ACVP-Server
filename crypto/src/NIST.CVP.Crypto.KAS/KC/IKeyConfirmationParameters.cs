using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.KC
{
    /// <summary>
    /// Interface describes the shared properties for KeyConfirmation parameters.
    /// </summary>
    public interface IKeyConfirmationParameters
    {
        /// <summary>
        /// This party's <see cref="KeyAgreementRole"/>
        /// </summary>
        KeyAgreementRole ThisPartyKeyAgreementRole { get; }
        /// <summary>
        /// This party's <see cref="KeyConfirmationRole"/>
        /// </summary>
        KeyConfirmationRole ThisPartyKeyConfirmationRole { get; }
        /// <summary>
        /// The type of key confirmation being performed.
        /// </summary>
        KeyConfirmationDirection KeyConfirmationType { get; }
        /// <summary>
        /// The <see cref="KeyConfirmationMacType"/> used in the MAC construction for the Key
        /// </summary>
        KeyConfirmationMacType MacType { get; }
        /// <summary>
        /// The keylength used in the MAC function
        /// </summary>
        int KeyLength { get; }
        /// <summary>
        /// THe length of the MAC to return
        /// </summary>
        int MacLength { get; }
        /// <summary>
        /// The ID of this key confirmation party (either party U or V)
        /// </summary>
        BitString ThisPartyIdentifier { get; }
        /// <summary>
        /// The ID of the other key confirmation party (either party U or V)
        /// </summary>
        BitString OtherPartyIdentifier { get; }
        /// <summary>
        /// The public key of this key confirmation party (either party U or V)
        /// </summary>
        BitString ThisPartyPublicKey { get; }
        /// <summary>
        /// The public key of the other key confirmation party (either party U or V)
        /// 
        /// Note in some Key Confirmation schemes, this public key can actually be a nonce (or null).
        /// Either case does not impact the implementation of the MACData that goes into the MAC generation.
        /// </summary>
        BitString OtherPartyPublicKey { get; }
        /// <summary>
        /// The Derived Keying Matierial - the key input of the MAC function
        /// </summary>
        BitString DerivedKeyingMaterial { get; }
        /// <summary>
        /// The Nonce used for Aes-Ccm
        /// </summary>
        BitString Nonce { get; }
    }
}