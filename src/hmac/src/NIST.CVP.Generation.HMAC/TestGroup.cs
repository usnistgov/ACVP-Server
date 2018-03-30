using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.HMAC
{
    public class TestGroup : ITestGroup
    {
        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroup(dynamic source)
        {
            TestGroupId = (int) source.tgId;
            TestType = source.testType;
            KeyLength = source.keyLen;
            MessageLength = source.msgLen;
            MacLength = source.macLen;
            Tests = new List<ITestCase>();
            foreach (var test in source.tests)
            {
                Tests.Add(new TestCase(test));
            }
        }

        public int TestGroupId { get; set; }
        [JsonProperty(PropertyName = "testType")]
        public string TestType { get; set; } = "AFT";
        [JsonProperty(PropertyName = "keyLen")]
        public int KeyLength { get; set; }
        [JsonProperty(PropertyName = "msgLen")]
        public int MessageLength { get; set; }
        [JsonProperty(PropertyName = "macLen")]
        public int MacLength { get; set; }
        public List<ITestCase> Tests { get; set; }

        [JsonIgnore]
        public ModeValues ShaMode { get; set; }
        [JsonIgnore]
        public DigestSizes ShaDigestSize { get; set; }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            int intVal = 0;
            if (!int.TryParse(value, out intVal))
            {
                return false;
            }

            // CAVS files in bytes, groups in bits
            switch (name.ToLower())
            {
                case "keylen":
                case "klen":
                    KeyLength = intVal * 8;
                    return true;
                case "msglen":
                case "mlen":
                    MessageLength = intVal * 8;
                    return true;
                case "maclen":
                case "tlen":
                    MacLength = intVal * 8;
                    return true;
            }
            return false;
        }
    }
}
