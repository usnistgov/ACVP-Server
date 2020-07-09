using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar3;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3;

namespace NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar3
{
	public class KasSscValResult
	{
		public ISecretKeyingMaterial ServerSecretKeyingMaterial { get; set; }
		public ISecretKeyingMaterial IutSecretKeyingMaterial { get; set; }
		public KasSscTestCaseExpectation Disposition { get; set; }
		public bool TestPassed { get; set; }
		public KeyAgreementResult SharedSecretComputationResult { get; set; }
	}
}