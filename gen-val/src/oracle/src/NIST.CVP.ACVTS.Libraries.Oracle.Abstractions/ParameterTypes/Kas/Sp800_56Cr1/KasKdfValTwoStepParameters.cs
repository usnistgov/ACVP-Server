using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfTwoStep;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Cr1
{
    public class KdaValTwoStepParameters
    {
        public KdaTestCaseDisposition Disposition { get; set; }
        public TwoStepConfiguration KdfConfiguration { get; set; }
        public TwoStepMultiExpansionConfiguration KdfConfigurationMultiExpand { get; set; }
        public int ZLength { get; set; }
    }
}
