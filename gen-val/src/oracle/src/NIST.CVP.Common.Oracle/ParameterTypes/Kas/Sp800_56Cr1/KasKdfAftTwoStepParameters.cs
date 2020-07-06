using NIST.CVP.Crypto.Common.KAS.KDF.KdfTwoStep;

namespace NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Cr1
{
	public class KasKdfAftTwoStepParameters
	{
		public TwoStepConfiguration KdfConfiguration { get; set; }
		public int ZLength { get; set; }
	}
}