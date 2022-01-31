using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfTwoStep;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Cr1
{
    public class KdaAftTwoStepParameters
    {
        public TwoStepConfiguration KdfConfiguration { get; set; }
        public TwoStepMultiExpansionConfiguration KdfConfigurationMultiExpand { get; set; }
        public int ZLength { get; set; }
    }
}
