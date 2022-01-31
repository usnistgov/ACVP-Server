using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.AnsiX942.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.ANSIX942
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; }

        public AnsiX942Types[] KdfType { get; set; }
        public MathDomain KeyLen { get; set; }
        public MathDomain OtherInfoLen { get; set; }   // Applies to concat only
        public MathDomain SuppInfoLen { get; set; }    // Applies to DER only, and each of the 4 fields match this length
        public MathDomain ZzLen { get; set; }
        public AnsiX942Oids[] Oid { get; set; }        // Applies to DER only
        public string[] HashAlg { get; set; }
    }
}
