using ACVPCore.Models.Capabilities;

namespace ACVPCore.Algorithms.Persisted
{
	public class AES_CCM : AlgorithmBase
	{

		[Property("key")]
		public NumericArrayCapability KeyLength { get; set; }

		[Property("tag")]
		public NumericArrayCapability TagLength { get; set; }

		[Property("iv")]
		public NumericArrayCapability IVLength { get; set; }

		[Property("pt")]
		public DomainCapability PayloadLength { get; set; }

		[Property("aad")]
		public DomainCapability AADLength { get; set; }

		public AES_CCM()
		{
			Name = "ACVP-AES-CCM";
		}

		public AES_CCM(ACVPCore.Algorithms.External.AES_CCM external) : this()
		{
			KeyLength = CreateNumericArrayCapability(external.KeyLength);
			TagLength = CreateNumericArrayCapability(external.TagLength);
			IVLength = CreateNumericArrayCapability(external.IVLength);
			PayloadLength = CreateDomainCapability(external.PayloadLength);
			AADLength = CreateDomainCapability(external.AADLength);
		}
	}
}
