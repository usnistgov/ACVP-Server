using System.Collections.Generic;
using ACVPCore.Algorithms.Persisted;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.ECDSA
{
	public class ECDSA_PKV : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "curve")]
		public List<string> Curves = new List<string>();

		public ECDSA_PKV(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "ECDSA", "keyVer")
		{
			string[] curves = new string[] { "P-192", "P-224", "P-256", "P-384", "P-521", "K-163", "K-233", "K-283", "K-409", "K-571", "B-163", "B-233", "B-283", "B-409", "B-571" };

			foreach (string curve in curves)
			{
				if (options.GetValue($"PKV_{curve}") == "True") Curves.Add(curve);
			}
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new ACVPCore.Algorithms.Persisted.ECDSAKeyVer186_4
		{
			Curves = Curves
		};
	}
}
