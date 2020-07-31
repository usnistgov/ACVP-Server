using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.Crypto.Common.KAS.Enums;

namespace NIST.CVP.Crypto.Common.KAS.KDF.KdfHkdf
{
	public class HkdfConfiguration : IKdfConfiguration
	{
		public KasKdf KdfType => KasKdf.Hkdf;
		public bool RequiresAdditionalNoncePair => false;
		public int L { get; set; }
		public int SaltLen { get; set; }
		public MacSaltMethod SaltMethod { get; set; }
		public string FixedInfoPattern { get; set; }
		public FixedInfoEncoding FixedInfoEncoding { get; set; }
		public HashFunctions HmacAlg { get; set; }
		public IKdfParameter GetKdfParameter(IKdfParameterVisitor visitor)
		{
			return visitor.CreateParameter(this);
		}
	}
}