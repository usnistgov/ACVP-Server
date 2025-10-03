using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.SP800_56Br2.DpComponent.TestCaseExpectations
{
    public class RsaDecryptExpectationProvider : TestCaseExpectationProviderBase<RsaDpDisposition>
    {
        public RsaDecryptExpectationProvider()
        {
            var expectationReasons = new List<RsaDpDisposition>
            {
                { RsaDpDisposition.CtEqual0, 3 }, 
                { RsaDpDisposition.CtEqual1, 3 }, 
                { RsaDpDisposition.CtEqualNMinusOne, 3 },
                { RsaDpDisposition.CtGreaterNMinusOne, 3 },
                { RsaDpDisposition.None, 3 }
            };

            LoadExpectationReasons(expectationReasons);
        }
    }
}
