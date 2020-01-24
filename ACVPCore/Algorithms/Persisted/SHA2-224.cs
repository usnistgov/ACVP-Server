using ACVPCore.Models.Capabilities;

namespace ACVPCore.Algorithms.Persisted
{
	public class SHA2_224 : AlgorithmBase
	{
		public DomainCapability MessageLength { get; set; }

		public SHA2_224()
		{
			Name = "SHA2-224";
		}

		public SHA2_224(ACVPCore.Algorithms.External.SHA2_224 external) : this()
		{
			MessageLength = CreateDomainCapability(external.MessageLength);
		}
	}
}
