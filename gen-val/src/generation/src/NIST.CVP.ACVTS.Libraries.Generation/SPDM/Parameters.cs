using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;

namespace NIST.CVP.ACVTS.Libraries.Generation.SPDM
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; }

        public MathDomain KeyLen { get; set; }
        public MathDomain THLen { get; set; }
        public SPDMVersions[] SPDMVersion { get; set; } = [];
        public bool[] UsesPSK { get; set; } = [];
        public HashFunctions[] HashAlgs { get; set; } = [];
    }
}
