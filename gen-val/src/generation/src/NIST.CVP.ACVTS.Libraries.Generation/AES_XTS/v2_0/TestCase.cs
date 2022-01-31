using System;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.AES;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_XTS.v2_0
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }

        [JsonIgnore]
        public bool? TestPassed => true;

        [JsonIgnore]
        public bool Deferred { get; set; }

        public TestGroup ParentGroup { get; set; }

        [JsonIgnore]
        public XtsKey XtsKey { get; set; }

        [JsonProperty(PropertyName = "key")]
        public BitString Key
        {
            get => XtsKey?.Key;
            set => XtsKey = new XtsKey(value);
        }

        [JsonProperty(PropertyName = "dataUnitLen")]
        public int DataUnitLen { get; set; }

        [JsonProperty(PropertyName = "payloadLen")]
        public int PayloadLen => PlainText.BitLength;

        [JsonProperty(PropertyName = "pt")]
        public BitString PlainText { get; set; }

        [JsonProperty(PropertyName = "ct")]
        public BitString CipherText { get; set; }

        [JsonProperty(PropertyName = "tweakValue")]
        public BitString I { get; set; }

        [JsonProperty(PropertyName = "sequenceNumber")]
        public int SequenceNumber { get; set; }
    }
}
