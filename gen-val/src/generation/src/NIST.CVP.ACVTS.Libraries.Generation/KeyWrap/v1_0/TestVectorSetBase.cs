using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.KeyWrap.v1_0
{
    public abstract class TestVectorSetBase<TTestGroup, TTestCase> : ITestVectorSet<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>
    {
        public int VectorSetId { get; set; }
        public abstract string Algorithm { get; set; }
        [JsonIgnore]
        public abstract string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public List<TTestGroup> TestGroups { get; set; } = new List<TTestGroup>();
    }
}
