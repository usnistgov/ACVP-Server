using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class FfcDomainParametersResult : IResult
    {
        public FfcDomainParameters DomainParameters { get; set; }
    }
}
