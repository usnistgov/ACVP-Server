using System.Collections.Generic;

namespace ACVPCore.Algorithms.Persisted
{
	public class AES_FF3_1 : PersistedAlgorithmBase
	{
		[AlgorithmProperty(Name = "direction", Type = AlgorithmPropertyType.StringArray)]
		public List<string> Direction { get; set; }

		[AlgorithmProperty(Name = "key", Type = AlgorithmPropertyType.NumberArray)]
		public List<int> KeyLength{ get; set; }

		[AlgorithmProperty(Name = "capabilities", Type = AlgorithmPropertyType.CompositeArray)]
		public List<Capability> Capabilities { get; set; }

		public AES_FF3_1()
		{
			Name = "ACVP-AES-FF3-1";
			Revision = "1.0";
		}

		public class Capability
		{
			[AlgorithmProperty(Name = "alphabet", Type = AlgorithmPropertyType.String)]
			public string Alphabet { get; set; }

			[AlgorithmProperty(Name = "radix", Type = AlgorithmPropertyType.Number)]
			public int Radix { get; set; }

			[AlgorithmProperty(Name = "minLen", Type = AlgorithmPropertyType.Number)]
			public int MinLength { get; set; }

			[AlgorithmProperty(Name = "maxLen", Type = AlgorithmPropertyType.Number)]
			public int MaxLength { get; set; }
		}

		public AES_FF3_1(ACVPCore.Algorithms.External.AES_FF3_1 external) : this()
		{
			Direction = external.Direction;
			KeyLength = external.KeyLength;
			Capabilities = new List<Capability>();

			foreach (var capability in external.Capabilities)
			{
				Capabilities.Add(new Capability
				{
					Alphabet = capability.Alphabet,
					Radix = capability.Radix,
					MinLength = capability.MinLength,
					MaxLength = capability.MaxLength
				});
			}
		}
	}
}
