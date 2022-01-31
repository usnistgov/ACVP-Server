using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.KeyWrap.v1_0
{
    public abstract class TestCaseBase<TTestGroup, TTestCase> : ITestCase<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>
    {
        public int TestCaseId { get; set; }
        public TTestGroup ParentGroup { get; set; }
        public bool? TestPassed { get; set; } = true;
        [JsonIgnore]
        public bool Deferred { get; set; }
        [JsonProperty(PropertyName = "key")]
        public BitString Key { get; set; }
        [JsonProperty(PropertyName = "pt")]
        public BitString PlainText { get; set; }
        [JsonProperty(PropertyName = "ct")]
        public BitString CipherText { get; set; }

        public abstract bool SetString(string name, string value);
    }
}
