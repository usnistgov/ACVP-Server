using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.EDDSA.v1_0.SigVer.TestCaseExpectations
{
    public class SignatureExpectationProvider : TestCaseExpectationProviderBase<EddsaSignatureDisposition>
    {
        public SignatureExpectationProvider()
        {
            var expectationReasons = new List<EddsaSignatureDisposition>
            {
                { EddsaSignatureDisposition.None, 3 },
                { EddsaSignatureDisposition.ModifyMessage, 3 },
                { EddsaSignatureDisposition.ModifyKey, 3 },
                { EddsaSignatureDisposition.ModifyR, 3 },
                { EddsaSignatureDisposition.ModifyS, 3 }
            };

            LoadExpectationReasons(expectationReasons);
        }
    }
}
