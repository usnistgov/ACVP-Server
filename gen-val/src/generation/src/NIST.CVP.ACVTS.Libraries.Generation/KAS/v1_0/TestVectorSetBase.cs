using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0
{
    public abstract class TestVectorSetBase<TTestGroup, TTestCase, TKasAlgoAttributes> : ITestVectorSet<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase, TKasAlgoAttributes>, new()
        where TTestCase : TestCaseBase<TTestGroup, TTestCase, TKasAlgoAttributes>, new()
        where TKasAlgoAttributes : IKasAlgoAttributes
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }

        public List<TTestGroup> TestGroups { get; set; } = new List<TTestGroup>();
    }
}
