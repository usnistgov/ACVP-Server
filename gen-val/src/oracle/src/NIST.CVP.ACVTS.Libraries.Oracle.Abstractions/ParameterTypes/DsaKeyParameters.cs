using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class DsaKeyParameters
    {
        public FfcDomainParameters DomainParameters { get; set; }
    }
}
