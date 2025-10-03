using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.EDDSA.v1_0.KeyVer.TestCaseExpectations
{
    public class KeyExpectationProvider : TestCaseExpectationProviderBase<EddsaKeyDisposition>
    {
        public KeyExpectationProvider()
        {
            var expectationReasons = new List<EddsaKeyDisposition>
            {
                { EddsaKeyDisposition.None, 5 }, 
                { EddsaKeyDisposition.NotOnCurve, 5 }
            };

            LoadExpectationReasons(expectationReasons);
        }
    }
}
