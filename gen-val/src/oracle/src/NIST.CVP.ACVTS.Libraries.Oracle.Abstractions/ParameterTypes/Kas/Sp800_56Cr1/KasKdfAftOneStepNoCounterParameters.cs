using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfOneStep;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Cr1
{
    public class KdaAftOneStepNoCounterParameters
    {
        public OneStepNoCounterConfiguration OneStepConfiguration { get; set; }
        public int ZLength { get; set; }
    }
}
