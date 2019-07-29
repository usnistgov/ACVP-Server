using System.Collections.Generic;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS.v1_0
{
    public abstract class TestVectorSetBase<TTestGroup, TTestCase, TKasDsaAlgoAttributes> : ITestVectorSet<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase, TKasDsaAlgoAttributes>, new()
        where TTestCase : TestCaseBase<TTestGroup, TTestCase, TKasDsaAlgoAttributes>, new()
        where TKasDsaAlgoAttributes : IKasDsaAlgoAttributes
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }

        public List<TTestGroup> TestGroups { get; set; } = new List<TTestGroup>();
    }
}