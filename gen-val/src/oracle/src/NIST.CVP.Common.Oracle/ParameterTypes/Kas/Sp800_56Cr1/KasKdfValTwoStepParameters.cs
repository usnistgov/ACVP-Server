using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfTwoStep;

namespace NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Cr1
{
	public class KasKdfValTwoStepParameters
	{
		public KasKdfTestCaseDisposition Disposition { get; set; }
		public TwoStepConfiguration KdfConfiguration { get; set; }
		public int ZLength { get; set; }
	}
}