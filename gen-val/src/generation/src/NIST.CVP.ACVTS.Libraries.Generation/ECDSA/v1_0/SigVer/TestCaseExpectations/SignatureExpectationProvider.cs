using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.ECDSA.v1_0.SigVer.TestCaseExpectations
{
    public class SignatureExpectationProvider : TestCaseExpectationProviderBase<EcdsaSignatureDisposition>
    {
        public SignatureExpectationProvider(bool isSample = false)
        {
            var expectationReasons = new List<EcdsaSignatureDisposition>();

            if (isSample)
            {
                expectationReasons.Add(EcdsaSignatureDisposition.None);
                expectationReasons.Add(EcdsaSignatureDisposition.ModifyMessage);
                expectationReasons.Add(EcdsaSignatureDisposition.ModifyKey);
                expectationReasons.Add(EcdsaSignatureDisposition.ModifyR);
                expectationReasons.Add(EcdsaSignatureDisposition.ModifyS);
                expectationReasons.Add(EcdsaSignatureDisposition.ZeroR);
                expectationReasons.Add(EcdsaSignatureDisposition.ZeroS);
            }
            else
            {
                expectationReasons.Add(EcdsaSignatureDisposition.None, 3);
                expectationReasons.Add(EcdsaSignatureDisposition.ModifyMessage, 3);
                expectationReasons.Add(EcdsaSignatureDisposition.ModifyKey, 3);
                expectationReasons.Add(EcdsaSignatureDisposition.ModifyR, 3);
                expectationReasons.Add(EcdsaSignatureDisposition.ModifyS, 3);
                expectationReasons.Add(EcdsaSignatureDisposition.ZeroR, 3);
                expectationReasons.Add(EcdsaSignatureDisposition.ZeroS, 3);
            }

            LoadExpectationReasons(expectationReasons);
        }
    }
}
