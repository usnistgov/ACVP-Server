using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;

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
        /// <returns>The derived key.</returns>
        KdfResult Kdf(KdfParameterOneStep param);
    }
}