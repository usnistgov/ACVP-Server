using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KES;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KTS
{
    /// <summary>
    /// Interface for RSA Optimal Asymmetric Encryption Padding (RSA-OAEP) as described in:
    /// https://nvlpubs.nist.gov/nistpubs/SpecialPublications/NIST.SP.800-56Br2.pdf section 7.2.2.
    /// </summary>
    public interface IRsaOaep
    {
        /// <summary>
        /// Create an encrypted shared secret based on <see cref="rsaPublicKey"/>, <see cref="keyingMaterial"/>, and
        /// <see cref="additionalInput"/>.
        /// </summary>
        /// <param name="rsaPublicKey">The other parties public key to use in the encryption of the padded, masked keying material.</param>
        /// <param name="keyingMaterial">(K) The keying material the will be encoded and encrypted.</param>
        /// <param name="additionalInput">(A) Context specific information to apply to the masking function.</param>
        /// <returns>An encrypted, masked ciphertext (containing a key) wrapped in a <see cref="SharedSecretResponse"/></returns>
        SharedSecretResponse Encrypt(PublicKey rsaPublicKey, BitString keyingMaterial, BitString additionalInput);
        /// <summary>
        /// Decrypt the encrypted, encoded <see cref="ciphertext"/> using the <see cref="rsaKeyPair"/> and <see cref="additionalInput"/>. 
        /// </summary>
        /// <param name="rsaKeyPair">The <see cref="KeyPair"/> to use in the decryption operation.</param>
        /// <param name="ciphertext">(C) The encoded encrypted ciphertext.</param>
        /// <param name="additionalInput">(A) The context specific information the went into the encryption operation.</param>
        /// <returns>The decrypted key.</returns>
        SharedSecretResponse Decrypt(KeyPair rsaKeyPair, BitString ciphertext, BitString additionalInput);
    }
}
