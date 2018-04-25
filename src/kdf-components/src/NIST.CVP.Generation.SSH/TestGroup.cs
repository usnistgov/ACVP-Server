using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Crypto.Common.KDF.Components.SSH.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.SSH
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public Cipher Cipher { get; set; }

        [JsonIgnore]
        public HashFunction HashAlg { get; set; }
        
        [JsonProperty(PropertyName = "hashAlg")]
        public string HashAlgName
        {
            get => HashAlg?.Name;
            set => HashAlg = ShaAttributes.GetHashFunctionFromName(value);
        }

        public string TestType { get; set; }
        public List<TestCase> Tests { get; set; } = new List<TestCase>();

        private int _ivLength;
        private int _keyLength;

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "hashalg":
                    HashAlg = ShaAttributes.GetHashFunctionFromName(value);
                    return true;

                case "iv length":
                    _ivLength = int.Parse(value);
                    return true;

                case "encryption key length":
                    _keyLength = int.Parse(value);
                    GetCipherFromInts();
                    return true;
            }

            return false;
        }

        private void GetCipherFromInts()
        {
            if (_ivLength == 64)
            {
                Cipher = Cipher.TDES;
                return;
            }

            if (_keyLength == 128)
            {
                Cipher = Cipher.AES128;
            }
            else if (_keyLength == 192)
            {
                Cipher = Cipher.AES192;
            }
            else
            {
                Cipher = Cipher.AES256;
            }
        }
    }
}
