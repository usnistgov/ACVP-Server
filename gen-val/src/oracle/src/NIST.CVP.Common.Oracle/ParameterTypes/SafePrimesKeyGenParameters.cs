using System.Numerics;
using NIST.CVP.Crypto.Common.KAS.SafePrimes.Enums;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
	public class SafePrimesKeyGenParameters : IParameters
	{
		public SafePrime SafePrime { get; set; }
	}
}