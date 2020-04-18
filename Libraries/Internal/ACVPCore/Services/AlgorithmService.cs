using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ACVPCore.Models;
using ACVPCore.Providers;

namespace ACVPCore.Services
{
	public class AlgorithmService : IAlgorithmService
	{
		IAlgorithmProvider _algorithmProvider;
		private static Dictionary<string, AlgorithmLookup> _algorithms;

		public AlgorithmService(IAlgorithmProvider algorithmProvider)
		{
			_algorithmProvider = algorithmProvider;

			//Get all the algorithms and add them to the dictionary
			_algorithms = _algorithmProvider.GetAlgorithms().ToDictionary(x => $"{x.Name}|{x.Mode}|{x.Revision}", x => x);
		}

		public long GetAlgorithmID(string name, string mode) => _algorithmProvider.GetAlgorithmID(name, mode);

		public AlgorithmLookup LookupAlgorithm(string name, string mode, string revision) =>_algorithms.TryGetValue($"{name}|{mode}|{revision}", out AlgorithmLookup algorithmLookup) ? algorithmLookup : null;

		public List<AlgorithmLookup> LookupAlgorithms(string name, string mode)
		{
			//This is to be used in prerequisite applicability lookups, where we only have the name and mode, not a revision. So we have to search for partial keys (name|mode|), and return a list because there are a few name/mode pairs where there are multiple revisions
			return _algorithms.Where(x => x.Key.StartsWith($"{name}|{mode}|")).Select(x => x.Value).ToList();
		}
	}
}
