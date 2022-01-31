using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.SafePrimes.Enums;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class SafePrimesKeyVerParameters
    {
        public SafePrime SafePrime { get; set; }
        public FfcDomainParameters DomainParameters { get; set; }
        public SafePrimesKeyDisposition Disposition { get; set; }
    }
}
