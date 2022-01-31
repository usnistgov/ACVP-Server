using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.TLS
{
    public interface ITlsKdf_v1_3
    {
        /// <summary>
        /// Compute the Early Secret and get all intermediate values associated with the computation.
        /// </summary>
        /// <param name="isExternalPsk">is the KDF running in external PSK mode or resumption PSK mode?</param>
        /// <param name="preSharedKey">The preshared key, can either be an external or resumption PSK.</param>
        /// <param name="clientHello">The nonce provided by the client to compute the early secret.</param>
        /// <returns>The derived early secret and intermediate values.</returns>
        TlsKdfV13EarlySecretResult GetDerivedEarlySecret(bool isExternalPsk, BitString preSharedKey, BitString clientHello);

        /// <summary>
        /// Compute the Handshake Secret and get all intermediate values associated with its computation.
        /// </summary>
        /// <param name="diffieHellmanSharedSecret">The shared secret as a result of (EC)DHE.</param>
        /// <param name="salt">The derived early secret.</param>
        /// <param name="clientHello">The hello nonce provided by the client.</param>
        /// <param name="serverHello">The hello nonce provided by the server.</param>
        /// <returns>The derived handshake secret and intermediate values.</returns>
        TlsKdfV13HandshakeSecretResult GetDerivedHandshakeSecret(BitString diffieHellmanSharedSecret, BitString salt,
            BitString clientHello, BitString serverHello);

        /// <summary>
        /// Compute the master secret and get all intermediate values associated with its computation.
        /// </summary>
        /// <param name="salt">the derived handshake secret.</param>
        /// <param name="clientHello">The hello nonce provided by the client.</param>
        /// <param name="serverHello">The hello nonce provided by the server.</param>
        /// <param name="serverFinished">The final nonce provided by the server.</param>
        /// <param name="clientFinished">The final nonce provided by the client.</param>
        /// <returns></returns>
        TlsKdfV13MasterSecretResult GetDerivedMasterSecret(BitString salt,
            BitString clientHello, BitString serverHello, BitString serverFinished, BitString clientFinished);

        /// <summary>
        /// Perform the full KDF from the individual parts.  Return all intermediate values.
        /// </summary>
        /// <param name="isExternalPsk">is the KDF running in external PSK mode or resumption PSK mode?</param>
        /// <param name="preSharedKey">The preshared key, can either be an external or resumption PSK.</param>
        /// <param name="diffieHellmanSharedSecret">The shared secret as a result of (EC)DHE.</param>
        /// <param name="clientHello">The nonce provided by the client.</param>
        /// <param name="serverHello">The nonce provided by the server.</param>
        /// <param name="serverFinished">The final nonce provided by the server.</param>
        /// <param name="clientFinished">The final nonce provided by the client.</param>
        /// <returns>The full KDF result and its intermediate values.</returns>
        TlsKdfV13FullResult GetFullKdf(bool isExternalPsk, BitString preSharedKey, BitString diffieHellmanSharedSecret,
            BitString clientHello, BitString serverHello, BitString serverFinished, BitString clientFinished);
    }
}
