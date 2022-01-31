using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.SafePrimes.Enums;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar3
{
    public class SafePrimeParameters : IParameters
    {
        public SafePrime SafePrime { get; set; }
    }
}
