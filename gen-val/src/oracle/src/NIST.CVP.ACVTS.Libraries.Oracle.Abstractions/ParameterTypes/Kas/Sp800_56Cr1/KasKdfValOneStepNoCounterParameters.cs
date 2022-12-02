using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfOneStep;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Cr1
{
    public class KdaValOneStepNoCounterParameters
    {
        public KdaTestCaseDisposition Disposition { get; set; }
        public OneStepNoCounterConfiguration OneStepConfiguration { get; set; }
        public int ZLength { get; set; }
    }
}
