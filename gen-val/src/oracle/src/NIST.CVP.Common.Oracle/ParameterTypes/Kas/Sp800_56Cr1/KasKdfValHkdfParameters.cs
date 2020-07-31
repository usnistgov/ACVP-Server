using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfHkdf;

namespace NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Cr1
{
	public class KasKdfValHkdfParameters
	{
		public KasKdfTestCaseDisposition Disposition { get; set; }
		public HkdfConfiguration KdfConfiguration { get; set; }
		public int ZLength { get; set; }
	}
}