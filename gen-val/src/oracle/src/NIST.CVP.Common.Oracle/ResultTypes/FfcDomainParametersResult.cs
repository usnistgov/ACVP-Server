using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class FfcDomainParametersResult : IResult
    {
        public FfcDomainParameters DomainParameters { get; set; }
    }
}