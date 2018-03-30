using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.MAC.CMAC.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.CMAC
{
    public abstract class TestGroupBase<TTestCase> : ITestGroup
        where TTestCase : TestCaseBase, new()
    {
        public TestGroupBase()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroupBase(dynamic source)
        {
            LoadSource(source);

        }

        public int TestGroupId { get; set; }
        [JsonProperty(PropertyName = "testType")]
        public string TestType { get; set; } = "AFT";
        [JsonProperty(PropertyName = "direction")]
        public string Function { get; set; }
        [JsonProperty(PropertyName = "keyLen")]
        public abstract int KeyLength { get; set; }
        [JsonProperty(PropertyName = "msgLen")]
        public int MessageLength { get; set; }
        [JsonProperty(PropertyName = "macLen")]
        public int MacLength { get; set; }
        public List<ITestCase> Tests { get; set; }

        [JsonIgnore]
        public CmacTypes CmacType { get; set; }

        protected abstract void LoadSource(dynamic source);

        public abstract bool SetString(string name, string value);

    }
}