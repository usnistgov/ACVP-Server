using NIST.CVP.Crypto.Common.KAS.SafePrimes.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SafePrimeGroups.v1_0.KeyGen
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; } = "SafePrimes";
        public string Mode { get; set; } = "KeyGen";
        public string Revision { get; set; } = "1.0";
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; }
        
        public SafePrime[] SafePrimeGroups { get; set; }
    }
}