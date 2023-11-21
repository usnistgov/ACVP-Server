using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.KDF.v1_0;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDA.Sp800_56Cr1.TwoStep
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; }
        public int L { get; set; }
        public MathDomain Z { get; set; }
        public TwoStepCapabilities[] Capabilities { get; set; }
    }

    public class TwoStepCapabilities : Capability
    {
        public MacSaltMethod[] MacSaltMethods { get; set; }
        /// <summary>
        /// The pattern used for FixedInputConstruction.
        /// </summary>
        public string FixedInfoPattern { get; set; }
        /// <summary>
        /// The encoding type of the fixedInput
        /// </summary>
        public FixedInfoEncoding[] Encoding { get; set; }
    }
}
