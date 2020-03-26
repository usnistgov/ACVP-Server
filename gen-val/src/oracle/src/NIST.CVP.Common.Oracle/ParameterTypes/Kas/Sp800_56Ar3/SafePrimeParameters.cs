using NIST.CVP.Crypto.Common.KAS.SafePrimes.Enums;

namespace NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Br2
{
    public class SafePrimeParameters : IParameters
    {
        public SafePrime SafePrime { get; set; }
    }
}