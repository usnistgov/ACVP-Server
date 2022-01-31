using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfOneStep;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Cr1
{
    public class KdaAftOneStepParameters
    {
        public OneStepConfiguration OneStepConfiguration { get; set; }
        public int ZLength { get; set; }
    }
}
