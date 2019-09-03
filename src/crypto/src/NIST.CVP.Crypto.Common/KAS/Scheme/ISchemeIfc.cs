namespace NIST.CVP.Crypto.Common.KAS.Scheme
{
    public interface ISchemeIfc
    {
        /// <summary>
        /// The options for the scheme, such as this party's role in the KAS, whether key confirmation occurs, etc.
        /// </summary>
        SchemeParametersIfc SchemeParameters { get; }
        /// <summary>
        /// The keying material to be used by this party.  Can be a key pair or a nonce.
        /// </summary>
        IIfcSecretKeyingMaterial ThisPartyKeyingMaterial { get; }
        /// <summary>
        /// Computes the KAS result based on party u/v contributions to the key, as well as kas scheme options.
        /// </summary>
        /// <param name="otherPartyKeyingMaterial">The other parties key contributions.</param>
        /// <returns>The result of the KAS operation.</returns>
        KasIfcResult ComputeResult(IIfcSecretKeyingMaterial otherPartyKeyingMaterial);
    }
}