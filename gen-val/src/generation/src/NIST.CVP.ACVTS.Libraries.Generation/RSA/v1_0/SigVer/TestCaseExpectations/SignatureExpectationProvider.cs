using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.SigVer.TestCaseExpectations
{
    public class SignatureExpectationProvider : TestCaseExpectationProviderBase<RSASignatureModifications>
    {
        public SignatureExpectationProvider()
        {
            var expectationReasons = new List<RSASignatureModifications>
            {
                RSASignatureModifications.Message,
                RSASignatureModifications.None,
                RSASignatureModifications.E,
                RSASignatureModifications.Signature,
                RSASignatureModifications.MoveIr,
                RSASignatureModifications.ModifyTrailer
            };

            LoadExpectationReasons(expectationReasons);
            
        }
    }
}
