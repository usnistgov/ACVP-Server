﻿using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.Common;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CFB8.v1_0
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        [JsonIgnore]
        public AlgoMode AlgoMode { get; set; }

        public string TestType { get; set; } = "AFT";
        public string InternalTestType { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "direction")]
        public string Function { get; set; }
        [JsonProperty(PropertyName = "keyLen")]
        public int KeyLength { get; set; }
        public List<TestCase> Tests { get; set; } = new List<TestCase>();

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            if (!int.TryParse(value, out var intVal))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "keylen":
                    KeyLength = intVal;
                    return true;
            }
            return false;
        }
    }
}