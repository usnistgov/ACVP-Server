using System.Collections.Generic;
using NIST.CVP.Algorithms.Persisted;

namespace LCAVPCore.Registration.Algorithms
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
