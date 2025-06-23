using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDA.Shared
{
    public class KdaExpectationProvider : TestCaseExpectationProviderBase<KdaTestCaseDisposition>
    {
        public KdaExpectationProvider(bool isSample)
        {
            var expectationReasons = new List<KdaTestCaseDisposition>();

            if (isSample)
            {
                var totalTests = 5;

                expectationReasons.Add(KdaTestCaseDisposition.Fail);
                expectationReasons.Add(KdaTestCaseDisposition.SuccessLeadingZeroNibble);

                expectationReasons.Add(KdaTestCaseDisposition.Success, totalTests - expectationReasons.Count);
            }
            else
            {
                var totalTests = 15;

                expectationReasons.Add(KdaTestCaseDisposition.Fail, 2);
                expectationReasons.Add(KdaTestCaseDisposition.SuccessLeadingZeroNibble, 2);

                expectationReasons.Add(KdaTestCaseDisposition.Success, totalTests - expectationReasons.Count);
            }

            LoadExpectationReasons(expectationReasons);
        }
    }
}
