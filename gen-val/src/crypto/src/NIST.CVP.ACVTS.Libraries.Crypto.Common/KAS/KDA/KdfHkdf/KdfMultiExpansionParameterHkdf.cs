using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfHkdf
{
    public class KdfMultiExpansionParameterHkdf : IKdfMultiExpansionParameter
    {
        public Kda KdfType => Kda.Hkdf;
        public BitString Z { get; set; }
        public BitString T { get; set; }
        /// <summary>
        /// The HmacAlg to use for the KDF
        /// </summary>
        public HashFunctions HmacAlg { get; set; }
        /// <summary>
        /// The salt used for the randomness extraction
        /// </summary>
        public BitString Salt { get; set; }
        public List<KdfMultiExpansionIterationParameter> IterationParameters { get; set; }
        public KdfMultiExpansionResult AcceptKdf(IKdfMultiExpansionVisitor visitor)
        {
            return visitor.Kdf(this);
        }
    }
}
