using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.PqgVer.TestCaseExpectations
{
    public class GExpectationProvider : TestCaseExpectationProviderBase<DsaGDisposition>
    {
        public GExpectationProvider(bool isSample = false)
        {
            var expectationReasons = new List<DsaGDisposition>();

            if (isSample)
            {
                expectationReasons.Add(DsaGDisposition.None);
                expectationReasons.Add(DsaGDisposition.ModifyG);
            }
            else
            {
                expectationReasons.Add(DsaGDisposition.None, 2);
                expectationReasons.Add(DsaGDisposition.ModifyG, 3);
            }

            LoadExpectationReasons(expectationReasons);
        }
    }
}
