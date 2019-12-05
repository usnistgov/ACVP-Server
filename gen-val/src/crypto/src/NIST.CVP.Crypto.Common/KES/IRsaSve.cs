using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KES
{
    /// <summary>
    /// Interface for RSA Secret Value Encapsulation as described in
    /// https://nvlpubs.nist.gov/nistpubs/SpecialPublications/NIST.SP.800-56Br2.pdf section 7.2
    /// </summary>
    public interface IRsaSve
    {
        /// <summary>
        /// Generate an encrypted <see cref="SharedSecretWithEncryptedValueResponse"/> whose value was encrypted using the provided <see cref="rsaPublicKey"/>.
        /// </summary>
        /// <param name="rsaPublicKey">The <see cref="PublicKey"/> to utilize for encrypting and generating the <see cref="SharedSecretWithEncryptedValueResponse"/>.</param>
        /// <returns>The encrypted and unencrypted secret keying material.</returns>
        SharedSecretWithEncryptedValueResponse Generate(PublicKey rsaPublicKey);

        /// <summary>
        /// Recover the secret keying material from <see cref="ciphertext"/> using the provided <see cref="rsaKeyPair"/>.
        /// </summary>
        /// <param name="rsaKeyPair">The keypair to use for performing the decryption.</param>
        /// <param name="ciphertext">The ciphertext to decrypt, arriving at the secret keying material.</param>
        /// <returns>The decrypted secret keying material.</returns>
        SharedSecretResponse Recover(KeyPair rsaKeyPair, BitString ciphertext);
    }
}