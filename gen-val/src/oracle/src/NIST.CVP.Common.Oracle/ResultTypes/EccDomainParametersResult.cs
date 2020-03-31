using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class EccDomainParametersResult : IResult
    {
        public EccDomainParameters EccDomainParameters { get; set; }
    }
}