using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Services
{
	public interface IAlgorithmService
	{
		long GetAlgorithmID(string name, string mode);
		AlgorithmLookup LookupAlgorithm(string name, string mode, string revision);
		List<AlgorithmLookup> LookupAlgorithms(string name, string mode);
	}
}