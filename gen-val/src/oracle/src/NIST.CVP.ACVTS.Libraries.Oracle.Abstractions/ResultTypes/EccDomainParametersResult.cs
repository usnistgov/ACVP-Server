using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class EccDomainParametersResult : IResult
    {
        public EccDomainParameters EccDomainParameters { get; set; }
    }
}
