using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_CBC_CTS.v1_0
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
