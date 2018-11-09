﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.AES_XTS
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; } = "AFT";
        public string InternalTestType { get; set; } = "AFT";
        [JsonProperty(PropertyName = "direction")]
        public string Direction { get; set; }
        [JsonProperty(PropertyName = "keyLen")]
        public int KeyLen { get; set; }
        [JsonProperty(PropertyName = "payloadLen")]
        public int PayloadLen { get; set; }
        [JsonProperty(PropertyName = "tweakMode")]
        public string TweakMode { get; set; }
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
                    KeyLen = int.Parse(value);
                    return true;

                case "dataunitlen":
                    PayloadLen = int.Parse(value);
                    return true;

                case "encrypt":
                    Direction = "encrypt";
                    return true;

                case "decrypt":
                    Direction = "decrypt";
                    return true;

                case "direction":
                    Direction = value;
                    return true;
            }
            return false;
        }
    }
}
