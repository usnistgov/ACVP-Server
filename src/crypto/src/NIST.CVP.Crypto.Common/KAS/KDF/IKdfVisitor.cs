using NIST.CVP.Crypto.Common.KAS.KDF.KdfIkeV1;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfIkeV2;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfTwoStep;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KAS.KDF
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
        /// Derive a key for Kas TwoStep.
        /// </summary>
        /// <param name="param">The parameters required for invoking a TwoStepKdf.</param>
        /// <param name="fixedInfo"> The fixed info constructed for the kdf.</param>
        /// <returns>The derived key.</returns>
        KdfResult Kdf(KdfParameterTwoStep param, BitString fixedInfo);
        /// <summary>
        /// Derive a key using IKE v1.
        /// </summary>
        /// <param name="param">The parameters required for invoking a IkeV1.</param>
        /// <param name="fixedInfo">Not used externally for this KDF.</param>
        /// <returns>The derived key.</returns>
        KdfResult Kdf(KdfParameterIkeV1 param, BitString fixedInfo = null);
        /// <summary>
        /// Derive a key using IKE v1.
        /// </summary>
        /// <param name="param">The parameters required for invoking a IkeV2.</param>
        /// <param name="fixedInfo">Not used externally for this KDF.</param>
        /// <returns>The derived key.</returns>
        KdfResult Kdf(KdfParameterIkeV2 param, BitString fixedInfo = null);
    }
}