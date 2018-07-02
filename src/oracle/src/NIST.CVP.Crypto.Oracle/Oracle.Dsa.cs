using System;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public DsaDomainParametersResult GetDsaDomainParameters() => throw new NotImplementedException();
        public VerifyResult<DsaDomainParametersResult> GetDsaDomainParametersVerify() => throw new NotImplementedException();
        public DsaKeyResult GetDsaKey() => throw new NotImplementedException();
        public VerifyResult<DsaKeyResult> GetDsaKeyVerify() => throw new NotImplementedException();
        public DsaSignatureResult GetDsaSignature() => throw new NotImplementedException();
        public VerifyResult<DsaSignatureResult> GetDsaVerifyResult() => throw new NotImplementedException();
    }
}
