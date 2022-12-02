using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA
{
    public interface IKdfMultiExpansionConfiguration
    {
        /// <summary>
        /// The KDF type (only some are supported for multi expansion).
        /// </summary>
        Kda KdfType { get; }
        /// <summary>
        /// The length of keying material to derive 
        /// </summary>
        public int L { get; set; }
        /// <summary>
        /// Utilize a <see cref="IKdfMultiExpansionParameterVisitor"/> to create a <see cref="IKdfMultiExpansionParameter"/> for use in a <see cref="IKdfMultiExpansion"/>
        /// </summary>
        /// <param name="visitor">The visitor for creating <see cref="IKdfMultiExpansionParameter"/>s.</param>
        /// <returns></returns>
        IKdfMultiExpansionParameter GetKdfParameter(IKdfMultiExpansionParameterVisitor visitor);
    }
}
