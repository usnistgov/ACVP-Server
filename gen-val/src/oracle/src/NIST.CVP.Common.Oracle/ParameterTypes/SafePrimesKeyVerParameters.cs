using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.KAS.SafePrimes.Enums;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class SafePrimesKeyVerParameters
    {
        public SafePrime SafePrime { get; set; }
        public FfcDomainParameters DomainParameters { get; set; }
        public SafePrimesKeyDisposition Disposition { get; set; }
    }
}