using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfHkdf;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Cr1
{
    public class KdaAftHkdfParameters
    {
        public HkdfConfiguration KdfConfiguration { get; set; }
        public HkdfMultiExpansionConfiguration KdfConfigurationMultiExpand { get; set; }
        public int ZLength { get; set; }
    }
}
