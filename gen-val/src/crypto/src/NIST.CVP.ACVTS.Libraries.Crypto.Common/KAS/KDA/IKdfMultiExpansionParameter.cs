using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA
{
    /// <summary>
    /// Interface for performing a KDF two step with multiple expansion.
    /// </summary>
    public interface IKdfMultiExpansionParameter
    {
        /// <summary>
        /// The KDF type (only some are supported for multi expansion).
        /// </summary>
        Kda KdfType { get; }

        /// <summary>
        /// The shared/derived secret.
        /// </summary>
        public BitString Z { get; set; }

        /// <summary>
        /// The optional auxiliary shared secret.
        /// </summary>
        public BitString T { get; set; }
        
        /// <summary>
        /// The "varying" parameters per expansion iteration.
        /// </summary>
        public List<KdfMultiExpansionIterationParameter> IterationParameters { get; }

        /// <summary>
        /// Performs the KDF making use of the visitor pattern.
        /// </summary>
        /// <param name="visitor">The visitor performing the KDF.</param>
        /// <returns></returns>
        KdfMultiExpansionResult AcceptKdf(IKdfMultiExpansionVisitor visitor);
    }

    /// <summary>
    /// Describes the per iteration differences for each expansion step of a multi expansion two step KDF.
    /// </summary>
    public class KdfMultiExpansionIterationParameter
    {
        /// <summary>
        /// The length of the keying material
        /// </summary>
        public int L { get; }
        /// <summary>
        /// The fixed info to use for the current iteration
        /// </summary>
        public BitString FixedInfo { get; }

        public KdfMultiExpansionIterationParameter(int l, BitString fixedInfo)
        {
            L = l;
            FixedInfo = fixedInfo;
        }
    }
}
