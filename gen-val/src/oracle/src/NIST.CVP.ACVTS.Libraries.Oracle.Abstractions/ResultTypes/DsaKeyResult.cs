using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class DsaKeyResult : IResult
    {
        public FfcKeyPair Key { get; set; }
    }
}
