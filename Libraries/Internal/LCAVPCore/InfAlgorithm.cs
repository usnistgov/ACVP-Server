using System.Collections.Generic;

namespace LCAVPCore
{
	public class InfAlgorithm
	{
		public string AlgorithmName { get; set; }
		public Dictionary<string, string> Options { get; set; }
		public Dictionary<string, string> Prerequisites { get; set; }

		public InfAlgorithm()
		{
			Options = new Dictionary<string, string>();
		}

		public InfAlgorithm(string name, IEnumerable<KeyValuePair<string, string>> options) : this()
		{
			AlgorithmName = name;
			Options.Add(options);
		}
	}
}