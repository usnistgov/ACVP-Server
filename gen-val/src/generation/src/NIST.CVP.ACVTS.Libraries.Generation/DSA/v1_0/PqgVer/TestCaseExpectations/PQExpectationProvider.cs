using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.PqgVer.TestCaseExpectations
{
    public class PQExpectationProvider : TestCaseExpectationProviderBase<DsaPQDisposition>
    {
        public PQExpectationProvider()
        {
            var expectationReasons = new List<DsaPQDisposition>
            {
                { DsaPQDisposition.None, 2 }, 
                DsaPQDisposition.ModifyP, 
                DsaPQDisposition.ModifyQ, 
                DsaPQDisposition.ModifySeed
            };

            LoadExpectationReasons(expectationReasons);
        }
    }
}
