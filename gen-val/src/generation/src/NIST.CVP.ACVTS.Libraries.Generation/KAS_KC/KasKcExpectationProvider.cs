using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_KC
{
    public class KasKcExpectationProvider : TestCaseExpectationProviderBase<KasKcDisposition>
    {
        public KasKcExpectationProvider(bool isSample)
        {
            var expectationReasons = new List<KasKcDisposition>();

            var totalCases = 25;
            var totalPerNonSuccessScenario = 4;
            if (isSample)
            {
                totalCases = 10;
                totalPerNonSuccessScenario = 1;
            }

            var totalSuccessScenarios = totalCases - totalPerNonSuccessScenario;

            expectationReasons.Add(KasKcDisposition.LeadingOneBitKey, totalPerNonSuccessScenario);
            expectationReasons.Add(KasKcDisposition.LeadingZeroByteKey, totalPerNonSuccessScenario);
            expectationReasons.Add(KasKcDisposition.LeadingZeroNibbleKey, totalPerNonSuccessScenario);

            expectationReasons.Add(KasKcDisposition.Success, totalSuccessScenarios);

            LoadExpectationReasons(expectationReasons);
        }
    }
}
