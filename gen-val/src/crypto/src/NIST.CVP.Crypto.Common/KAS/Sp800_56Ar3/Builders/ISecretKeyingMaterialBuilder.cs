using NIST.CVP.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3.Builders
{
    /// <summary>
    /// Interface for building secret keying material for various KAS schemes.
    /// </summary>
    public interface ISecretKeyingMaterialBuilder
    {
        /// <summary>
        /// Set the domain parameters used for key generation.
        /// </summary>
        /// <param name="value">The domain parameters.</param>
        /// <returns>This builder.</returns>
        ISecretKeyingMaterialBuilder WithDomainParameters(IDsaDomainParameters value);
        /// <summary>
        /// Set the party ephemeral key. 
        /// </summary>
        /// <param name="value">The ephemeral key.</param>
        /// <returns>This builder.</returns>
        ISecretKeyingMaterialBuilder WithEphemeralKey(IDsaKeyPair value);
        /// <summary>
        /// Set the party static key. 
        /// </summary>
        /// <param name="value">The static key.</param>
        /// <returns>This builder.</returns>
        ISecretKeyingMaterialBuilder WithStaticKey(IDsaKeyPair value);
        /// <summary>
        /// Set the party ephemeral nonce. 
        /// </summary>
        /// <param name="value">The ephemeral nonce.</param>
        /// <returns>This builder.</returns>
        ISecretKeyingMaterialBuilder WithEphemeralNonce(BitString value);
        /// <summary>
        /// Set the party dkm nonce. 
        /// </summary>
        /// <param name="value">The dkm nonce.</param>
        /// <returns>This builder.</returns>
        ISecretKeyingMaterialBuilder WithDkmNonce(BitString value);
        /// <summary>
        /// Set the party identifier. 
        /// </summary>
        /// <param name="value">The party identifier.</param>
        /// <returns>This builder.</returns>
        ISecretKeyingMaterialBuilder WithPartyId(BitString value);
        /// <summary>
        /// Construct the <see cref="ISecretKeyingMaterial"/>
        /// </summary>
        /// <param name="scheme">The scheme being used.</param>
        /// <param name="kasMode">The kas mode.</param>
        /// <param name="thisPartyKeyAgreementRole">This party key agreement role.</param>
        /// <param name="keyConfirmationRole">This party key confirmation role.</param>
        /// <param name="keyConfirmationDirection">The key confirmation direction.</param>
        /// <returns>The secret keying material.</returns>
        ISecretKeyingMaterial Build(
            KasScheme scheme, 
            KasMode kasMode, 
            KeyAgreementRole thisPartyKeyAgreementRole, 
            KeyConfirmationRole keyConfirmationRole, 
            KeyConfirmationDirection keyConfirmationDirection);
    }
}