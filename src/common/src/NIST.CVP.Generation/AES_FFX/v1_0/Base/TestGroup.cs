using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.Common;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_FFX.v1_0.Base
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; } = "AFT";
        [JsonProperty(PropertyName = "direction")]
        public BlockCipherDirections Function { get; set; }
        [JsonProperty(PropertyName = "keyLen")]
        public int KeyLength { get; set; }

        public string Alphabet => Capability?.Alphabet;
        public int Radix => Capability?.Radix ?? 0;
        
        [JsonIgnore] public AlgoMode AlgoMode { get; set; }
        [JsonIgnore] public Capability Capability { get; set; }

        public List<TestCase> Tests { get; set; } = new List<TestCase>();

    }
}
