namespace NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3
{
    public interface IScheme
    {
        /// <summary>
        /// The properties of the KAS instance as they relate to scheme, role, key confirmation, ID, etc.
        /// </summary>
        SchemeParameters SchemeParameters { get; }
        /// <summary>
        /// The keying material to be used by this party.
        /// </summary>
        ISecretKeyingMaterial ThisPartyKeyingMaterial { get; set; }
        /// <summary>
        /// Computes the KAS result based on party u/v contributions to the key, as well as kas scheme options.
        /// </summary>
        /// <param name="otherPartyKeyingMaterial">The other parties key contributions.</param>
        /// <returns>The result of the KAS operation.</returns>
        KeyAgreementResult ComputeResult(ISecretKeyingMaterial otherPartyKeyingMaterial);
    }
}