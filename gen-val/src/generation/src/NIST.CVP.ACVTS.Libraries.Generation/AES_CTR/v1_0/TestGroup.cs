using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_CTR.v1_0
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; } = "AFT";
        public string InternalTestType { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "direction")]
        public string Direction { get; set; }
        [JsonProperty(PropertyName = "keyLen")]
        public int KeyLength { get; set; }

        // Properties for specific groups
        [JsonIgnore]
        public MathDomain PayloadLength { get; set; }

        // This is a vectorset / IUT property but it needs to be defined somewhere other than Parameter.cs
        [JsonProperty(PropertyName = "incremental")]
        public bool IncrementalCounter { get; set; }
        [JsonProperty(PropertyName = "overflow")]
        public bool OverflowCounter { get; set; }

        #region RFC testing properties
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IvGenModes IvGenMode { get; set; }

        /// <summary>
        /// Requires a specific IV construction (IV is made up of 3 parts iv||nonce||blockCounter
        /// where blockCounter starts as a 32 bit string representing the values of 1 (00000001 in hex). 
        /// </summary>
        public bool RfcTestMode { get; set; }
        #endregion RFC testing properties

        public List<TestCase> Tests { get; set; } = new List<TestCase>();

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "keylen":
                    KeyLength = Int32.Parse(value);
                    return true;

                case "direction":
                    Direction = value;
                    return true;
            }
            return false;
        }
    }
}
