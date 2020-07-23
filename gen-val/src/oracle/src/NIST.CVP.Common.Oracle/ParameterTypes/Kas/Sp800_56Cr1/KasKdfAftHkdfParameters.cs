using NIST.CVP.Crypto.Common.KAS.KDF.KdfHkdf;

namespace NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Cr1
{
	public class KasKdfAftHkdfParameters
	{
		public HkdfConfiguration KdfConfiguration { get; set; }
		public int ZLength { get; set; }
	}
}