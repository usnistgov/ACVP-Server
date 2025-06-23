using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.SigVer.TestCaseExpectations
{
    public class SignatureExpectationProvider : TestCaseExpectationProviderBase<DsaSignatureDisposition>
    {
        public SignatureExpectationProvider(bool isSample = false)
        {
            var expectationReasons = new List<DsaSignatureDisposition>();

            if (isSample)
            {
                expectationReasons.Add(DsaSignatureDisposition.None);
                expectationReasons.Add(DsaSignatureDisposition.ModifyMessage);
                expectationReasons.Add(DsaSignatureDisposition.ModifyKey);
                expectationReasons.Add(DsaSignatureDisposition.ModifyR);
                expectationReasons.Add(DsaSignatureDisposition.ModifyS);
            }
            else
            {
                expectationReasons.Add(DsaSignatureDisposition.None, 7);
                expectationReasons.Add(DsaSignatureDisposition.ModifyMessage, 2);
                expectationReasons.Add(DsaSignatureDisposition.ModifyKey, 2);
                expectationReasons.Add(DsaSignatureDisposition.ModifyR, 2);
                expectationReasons.Add(DsaSignatureDisposition.ModifyS, 2);
            }

            LoadExpectationReasons(expectationReasons);
        }
    }
}
