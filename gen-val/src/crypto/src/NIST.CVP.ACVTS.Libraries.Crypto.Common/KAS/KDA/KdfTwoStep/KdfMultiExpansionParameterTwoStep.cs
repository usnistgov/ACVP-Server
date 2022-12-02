using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfTwoStep
{
    public class KdfMultiExpansionParameterTwoStep : IKdfMultiExpansionParameter
    {
        public Kda KdfType => Kda.TwoStep;
        /// <summary>
        /// The TwoStep KDF mode
        /// </summary>
        public KdfModes KdfMode { get; set; }
        /// <summary>
        /// The MAC used for the KDF.
        /// </summary>
        public MacModes MacMode { get; set; }
        /// <summary>
        /// Where the counter is located within the data fed into the KDF.
        /// </summary>
        public CounterLocations CounterLocation { get; set; }
        /// <summary>
        /// The length of the counter
        /// </summary>
        public int CounterLen { get; set; }
        /// <summary>
        /// The salt used in the randomness extraction step 
        /// </summary>
        public BitString Salt { get; set; }
        /// <summary>
        /// The iv used in some modes of the KDF
        /// </summary>
        public BitString Iv { get; set; }
        public BitString Z { get; set; }
        public BitString T { get; set; }
        public List<KdfMultiExpansionIterationParameter> IterationParameters { get; set; }
        public KdfMultiExpansionResult AcceptKdf(IKdfMultiExpansionVisitor visitor)
        {
            return visitor.Kdf(this);
        }
    }
}
