using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.HMAC
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }

        public string TestType { get; set; }

        [JsonProperty(PropertyName = "keyLen")]
        public int KeyLength { get; set; }

        [JsonProperty(PropertyName = "msgLen")]
        public int MessageLength { get; set; }

        [JsonProperty(PropertyName = "macLen")]
        public int MacLength { get; set; }

        public List<TestCase> Tests { get; set; } = new List<TestCase>();

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

            if (!int.TryParse(value, out var intVal))
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
