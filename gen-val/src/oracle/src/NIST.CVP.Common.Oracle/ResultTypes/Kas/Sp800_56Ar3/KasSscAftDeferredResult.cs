using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3;

namespace NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar3
{
	public class KasSscAftDeferredResult
	{
		public ISecretKeyingMaterial ServerSecretKeyingMaterial { get; set; }
		public ISecretKeyingMaterial IutSecretKeyingMaterial { get; set; }
		public KeyAgreementResult KasResult { get; set; }
	}
}