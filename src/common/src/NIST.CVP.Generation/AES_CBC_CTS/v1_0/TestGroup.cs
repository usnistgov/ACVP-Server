using Newtonsoft.Json;
using NIST.CVP.Common;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;
using System.Collections.Generic;

namespace NIST.CVP.Generation.AES_CBC_CTS.v1_0
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; } = "AFT";
        public string InternalTestType { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "direction")]
        public string Function { get; set; }
        [JsonProperty(PropertyName = "keyLen")]
        public int KeyLength { get; set; }

        public List<TestCase> Tests { get; set; } = new List<TestCase>();

        [JsonIgnore] public bool IsPartialBlockGroup { get; set; }
        [JsonIgnore] public MathDomain PayloadLen { get; set; }
        [JsonIgnore] public AlgoMode AlgoMode { get; set; }

        [JsonIgnore]
        public BlockCipherModesOfOperation BlockCipherModeOfOperation =>
            SpecificationToDomainMapping.GetModeOfOperationFromAlgoMode(AlgoMode);
    }
}
