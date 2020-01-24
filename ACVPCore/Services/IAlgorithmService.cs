using ACVPCore.Models;

namespace ACVPCore.Services
{
	public interface IAlgorithmService
	{
		long GetAlgorithmID(string name, string mode);
		AlgorithmLookup LookupAlgorithm(string name, string mode, string revision);
	}
}