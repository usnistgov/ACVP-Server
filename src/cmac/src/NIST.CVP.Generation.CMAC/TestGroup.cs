using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.MAC.CMAC.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.CMAC
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; }
        [JsonProperty(PropertyName = "direction")]
        public string Function { get; set; }
        [JsonProperty(PropertyName = "keyLen")]
        public int KeyLength { get; set; }
        [JsonProperty(PropertyName = "msgLen")]
        public int MessageLength { get; set; }
        [JsonProperty(PropertyName = "macLen")]
        public int MacLength { get; set; }
        public List<TestCase> Tests { get; set; } = new List<TestCase>();

        [JsonIgnore]
        public CmacTypes CmacType { get; set; }

        #region Tdes only
        [JsonProperty(PropertyName = "keyingOption", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int KeyingOption { get; set; }
        #endregion Tdes only

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
                case "klen":
                    switch (intVal)
                    {
                        case 3:
                            KeyingOption = 1;
                            CmacType = CmacTypes.TDES;
                            break;
                        case 2:
                            KeyingOption = 2;
                            CmacType = CmacTypes.TDES;
                            break;
                        default:
                            KeyLength = intVal;
                            CmacType = CmacTypes.AES128;
                            break;
                    }
                    return true;
                case "msglen":
                case "mlen":
                    MessageLength = intVal;
                    return true;
                case "maclen":
                case "tlen":
                    MacLength = intVal;
                    return true;
            }
            return false;
        }
    }
}