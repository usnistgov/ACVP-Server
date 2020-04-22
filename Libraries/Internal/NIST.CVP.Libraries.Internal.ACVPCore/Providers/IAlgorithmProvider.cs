using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Providers
{
	public interface IAlgorithmProvider
	{
		long GetAlgorithmID(string name, string mode);
		List<AlgorithmLookup> GetAlgorithms();
	}
}