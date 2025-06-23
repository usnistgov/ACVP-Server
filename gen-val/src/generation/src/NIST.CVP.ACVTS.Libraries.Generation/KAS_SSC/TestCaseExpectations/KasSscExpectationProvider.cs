using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_SSC.TestCaseExpectations
{
    public class KasSscExpectationProvider : TestCaseExpectationProviderBase<KasSscTestCaseExpectation>
    {
        public KasSscExpectationProvider(bool isSample, bool includeFailureTests)
        {
            var expectationReasons = new List<KasSscTestCaseExpectation>();

            if (isSample)
            {
                var totalTests = 5;

                if (includeFailureTests)
                {
                    expectationReasons.Add(KasSscTestCaseExpectation.FailChangedZ);
                    expectationReasons.Add(KasSscTestCaseExpectation.PassLeadingZeroNibble);
                }

                expectationReasons.Add(KasSscTestCaseExpectation.Pass, totalTests - expectationReasons.Count);
            }
            else
            {
                var totalTests = 15;

                if (includeFailureTests)
                {
                    expectationReasons.Add(KasSscTestCaseExpectation.FailChangedZ, 5);
                    expectationReasons.Add(KasSscTestCaseExpectation.PassLeadingZeroNibble, 1);
                }

                expectationReasons.Add(KasSscTestCaseExpectation.Pass, totalTests - expectationReasons.Count);
            }

            LoadExpectationReasons(expectationReasons);
        }
    }
}
