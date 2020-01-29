using System.Collections.Generic;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.Persisted
{
	public class SHA2_224 : PersistedAlgorithmBase
	{
		[AlgorithmProperty(Name = "digestSize", Type = AlgorithmPropertyType.StringArray)]
		public List<string> DigestSize { get; set; }

		[AlgorithmProperty(Name = "key", Type = AlgorithmPropertyType.NumberArray)]
		public Domain MessageLength { get; set; }

		public SHA2_224()
		{
			Name = "SHA2-224";
		}

		public SHA2_224(ACVPCore.Algorithms.External.SHA2_224 external) : this()
		{
			MessageLength = external.MessageLength;
			DigestSize = new List<string> { "224" };
		}
	}
}
