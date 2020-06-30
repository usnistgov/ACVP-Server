using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;

namespace NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Cr1
{
	public class KasKdfValOneStepParameters
	{
		public KasKdfTestCaseDisposition Disposition { get; set; }
		public OneStepConfiguration OneStepConfiguration { get; set; }
		public int ZLength { get; set; }
	}
}