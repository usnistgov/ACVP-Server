using System.Collections;
using ACVPCore.Models;
using ACVPCore.Providers;

namespace ACVPCore.Services
{
	public class AlgorithmService : IAlgorithmService
	{
		IAlgorithmProvider _algorithmProvider;
		private static Hashtable _algorithms;

		public AlgorithmService(IAlgorithmProvider algorithmProvider)
		{
			_algorithmProvider = algorithmProvider;

			//Get all the algorithms and add them to the hash table
			_algorithmProvider.GetAlgorithms().ForEach(x => _algorithms.Add($"{x.Name}|{x.Mode}|{x.Revision}", x));
		}

		public long GetAlgorithmID(string name, string mode) => _algorithmProvider.GetAlgorithmID(name, mode);

		public AlgorithmLookup LookupAlgorithm(string name, string mode, string revision) => (AlgorithmLookup)_algorithms[$"{name}|{mode}|{revision}"];
	}
}
