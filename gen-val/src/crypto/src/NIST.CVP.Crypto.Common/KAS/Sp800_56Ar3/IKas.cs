namespace NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3
{
    public interface IKas
    {
        /// <summary>
        /// The KAS Scheme utilized
        /// </summary>
        IScheme Scheme { get; }
        /// <summary>
        /// Establish a key between this party and the other party.
        /// </summary>
        /// <param name="otherPartyKeyingMaterial">The other parties keying material that contributes to the key.</param>
        /// <returns>A <see cref="KasResult"/> that contains the result of the KAS attempt.</returns>
        KeyAgreementResult ComputeResult(ISecretKeyingMaterial otherPartyKeyingMaterial);
    }
}