using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar3;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Ar3
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
