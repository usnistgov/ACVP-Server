using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.SafePrimes.Enums;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class SafePrimesKeyGenParameters : IParameters
    {
        public SafePrime SafePrime { get; set; }
    }
}
