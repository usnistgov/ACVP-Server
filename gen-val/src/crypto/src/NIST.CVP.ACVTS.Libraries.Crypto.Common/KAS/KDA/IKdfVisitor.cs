using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfHkdf;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfIkeV1;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfIkeV2;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfOneStep;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfTls10_11;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfTls12;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfTwoStep;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA
{
    /// <summary>
    /// Defines different KDF parameter types.
    /// </summary>
    public interface IKdfVisitor
    {
        /// <summary>
        /// Derive a key for Kas OneStep.
        /// </summary>
        /// <param name="param">The parameters required for invoking a OneStepKdf.</param>
        /// <param name="fixedInfo"> The fixed info constructed for the kdf.</param>
        /// <returns>The derived key.</returns>
        KdfResult Kdf(KdfParameterOneStep param, BitString fixedInfo);
        /// <summary>
        /// Derive a key for Kas OneStep w/o counter.
        /// </summary>
        /// <param name="param">The parameters required for invoking a OneStepKdfNoCounter.</param>
        /// <param name="fixedInfo"> The fixed info constructed for the kdf.</param>
        /// <returns>The derived key.</returns>
        KdfResult Kdf(KdfParameterOneStepNoCounter param, BitString fixedInfo);
        /// <summary>
        /// Derive a key for Kas TwoStep.
        /// </summary>
        /// <param name="param">The parameters required for invoking a TwoStepKdf.</param>
        /// <param name="fixedInfo"> The fixed info constructed for the kdf.</param>
        /// <returns>The derived key.</returns>
        KdfResult Kdf(KdfParameterTwoStep param, BitString fixedInfo);
        /// <summary>
        /// Derive a key for Kas HKDF.
        /// </summary>
        /// <param name="param">The parameters required for invoking a HKDF.</param>
        /// <param name="fixedInfo"> The fixed info constructed for the kdf.</param>
        /// <returns>The derived key.</returns>
        KdfResult Kdf(KdfParameterHkdf param, BitString fixedInfo);
        /// <summary>
        /// Derive a key using IKE v1.
        /// </summary>
        /// <param name="param">The parameters required for invoking a IkeV1.</param>
        /// <param name="fixedInfo">Not used externally for this KDF.</param>
        /// <returns>The derived key.</returns>
        KdfResult Kdf(KdfParameterIkeV1 param, BitString fixedInfo = null);
        /// <summary>
        /// Derive a key using IKE v2.
        /// </summary>
        /// <param name="param">The parameters required for invoking a IkeV2.</param>
        /// <param name="fixedInfo">Not used externally for this KDF.</param>
        /// <returns>The derived key.</returns>
        KdfResult Kdf(KdfParameterIkeV2 param, BitString fixedInfo = null);
        /// <summary>
        /// Derive a key using TLS v1.0/v1.1.
        /// </summary>
        /// <param name="param">The parameters required for invoking TLS v1.0/v1.1.</param>
        /// <param name="fixedInfo">Not used externally for this KDF.</param>
        /// <returns>The derived key.</returns>
        KdfResult Kdf(KdfParameterTls10_11 param, BitString fixedInfo = null);
        /// <summary>
        /// Derive a key using TLS v1.2.
        /// </summary>
        /// <param name="param">The parameters required for invoking TLS v1.2</param>
        /// <param name="fixedInfo">Not used externally for this KDF.</param>
        /// <returns>The derived key.</returns>
        KdfResult Kdf(KdfParameterTls12 param, BitString fixedInfo = null);
    }
}
