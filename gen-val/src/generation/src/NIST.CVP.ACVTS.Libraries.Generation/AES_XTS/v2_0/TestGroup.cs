using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_XTS.v2_0
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; } = "AFT";
        public string InternalTestType { get; set; }

        [JsonProperty(PropertyName = "direction")]
        public BlockCipherDirections Direction { get; set; }

        [JsonProperty(PropertyName = "keyLen")]
        public int KeyLen { get; set; }

        [JsonIgnore]
        public MathDomain PayloadLen { get; set; }

        [JsonIgnore]
        public MathDomain DataUnitLen { get; set; }

        [JsonProperty(PropertyName = "tweakMode")]
        public XtsTweakModes TweakMode { get; set; }

        public List<TestCase> Tests { get; set; } = new List<TestCase>();
    }
}
