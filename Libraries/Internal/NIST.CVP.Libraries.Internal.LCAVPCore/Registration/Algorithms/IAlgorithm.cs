using System.Collections.Generic;
using NIST.CVP.Libraries.Internal.Algorithms.Persisted;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Registration.Algorithms
{
	public interface IAlgorithm
	{
		//This is intentionally empty
		string Algorithm { get; }
		string Mode { get; }
		string Revision { get; }
		List<Prerequisite> CleanPreReqs { get; }
		int AlgorithmID { get; }
		string Family { get; }
		List<string> Errors { get; }

		IPersistedAlgorithm ToPersistedAlgorithm();
	}
}
