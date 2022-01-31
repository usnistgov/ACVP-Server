using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.SafePrimes.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.SafePrimeGroups.v1_0.KeyVer
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; } = "safePrimes";
        public string Mode { get; set; } = "keyVer";
        public string Revision { get; set; } = "1.0";
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; }

        public SafePrime[] SafePrimeGroups { get; set; }
    }
}
