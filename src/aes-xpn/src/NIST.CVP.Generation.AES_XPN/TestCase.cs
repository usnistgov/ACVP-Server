﻿using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_XPN
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public bool? TestPassed { get; set; }
        public bool Deferred { get; set; }
        public TestGroup ParentGroup { get; set; }
        [JsonProperty(PropertyName = "plainText")]
        public BitString PlainText { get; set; }
        [JsonProperty(PropertyName = "key")]
        public BitString Key { get; set; }
        [JsonProperty(PropertyName = "aad")]
        public BitString AAD { get; set; }
        [JsonProperty(PropertyName = "iv")]
        public BitString IV { get; set; }
        [JsonProperty(PropertyName = "salt")]
        public BitString Salt { get; set; }
        [JsonProperty(PropertyName = "cipherText")]
        public BitString CipherText { get; set; }
        [JsonProperty(PropertyName = "tag")]
        public BitString Tag { get; set; }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }
      
            switch (name.ToLower())
            {
                case "key":
                    Key = new BitString(value);
                    return true;
                case "aad":
                    AAD = new BitString(value);
                    return true;
                case "tag":
                    Tag= new BitString(value);
                    return true;
                case "iv":
                    IV = new BitString(value);
                    return true;
                case "salt":
                    Salt = new BitString(value);
                    return true;
                case "plaintext":
                case "pt":
                    PlainText= new BitString(value);
                    return true;
                case "ciphertext":
                case "ct":
                    CipherText = new BitString(value);
                    return true;
            }
            return false;
        }
    }
}