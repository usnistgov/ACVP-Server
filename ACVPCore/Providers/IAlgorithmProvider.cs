using System.Collections.Generic;
using ACVPCore.Models;

namespace ACVPCore.Providers
{
	public interface IAlgorithmProvider
	{
		long GetAlgorithmID(string name, string mode);
		List<AlgorithmLookup> GetAlgorithms();
	}
}