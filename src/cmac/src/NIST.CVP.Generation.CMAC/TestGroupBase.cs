using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.Common.MAC.CMAC.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.CMAC
{
    public abstract class TestGroupBase<TTestGroup, TTestCase> : ITestGroup<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase>
        where TTestCase : TestCaseBase<TTestGroup, TTestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; }
        [JsonProperty(PropertyName = "direction")]
        public string Function { get; set; }
        [JsonProperty(PropertyName = "keyLen")]
        public abstract int KeyLength { get; set; }
        [JsonProperty(PropertyName = "msgLen")]
        public int MessageLength { get; set; }
        [JsonProperty(PropertyName = "macLen")]
        public int MacLength { get; set; }
        public List<TTestCase> Tests { get; set; } = new List<TTestCase>();

        [JsonIgnore]
        public CmacTypes CmacType { get; set; }

        public abstract bool SetString(string name, string value);
    }
}