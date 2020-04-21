using System.Collections.Generic;
using NIST.CVP.Libraries.Internal.ACVPCore.Models;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Providers
{
	public interface IAlgorithmProvider
	{
		long GetAlgorithmID(string name, string mode);
		List<AlgorithmLookup> GetAlgorithms();
	}
}