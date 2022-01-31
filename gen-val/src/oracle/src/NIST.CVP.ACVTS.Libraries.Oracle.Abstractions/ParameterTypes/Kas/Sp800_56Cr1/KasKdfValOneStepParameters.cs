using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Cr1
{
    public class KdaValOneStepParameters
    {
        public KdaTestCaseDisposition Disposition { get; set; }
        public OneStepConfiguration OneStepConfiguration { get; set; }
        public int ZLength { get; set; }
    }
}
