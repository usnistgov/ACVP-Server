﻿using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.SHA3.v1_0
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public List<TestCase> Tests { get; set; } = new List<TestCase>();

        [JsonProperty(PropertyName = "testType")]
        public string TestType { get; set; }

        [JsonIgnore]
        public string Function { get; set; }

        [JsonIgnore]
        public int DigestSize { get; set; }

        [JsonProperty(PropertyName = "inBit")]
        public bool BitOrientedInput { get; set; } = false;

        [JsonProperty(PropertyName = "outBit")]
        public bool BitOrientedOutput { get; set; } = false;

        [JsonProperty(PropertyName = "inEmpty")]
        public bool IncludeNull { get; set; } = false;

        [JsonIgnore]
        public MathDomain OutputLength { get; set; }

        [JsonProperty(PropertyName = "maxOutLen")]
        public int MaxOutputLength => OutputLength?.GetDomainMinMax().Maximum ?? 0;

        [JsonProperty(PropertyName = "minOutLen")]
        public int MinOutputLength => OutputLength?.GetDomainMinMax().Minimum ?? 0;

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value))
            {
                return false;
            }

            name = name.ToLower();

            switch (name)
            {
                case "testtype":
                    TestType = value;
                    return true;
                case "function":
                    Function = value;
                    return true;
            }

            return false;
        }
    }
}