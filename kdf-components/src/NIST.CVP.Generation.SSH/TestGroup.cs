using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json.Linq;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Crypto.Common.KDF.Components.SSH.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.SSH
{
    public class TestGroup : ITestGroup
    {
        public Cipher Cipher { get; set; }
        public HashFunction HashAlg { get; set; }

        private int _ivLength;
        private int _keyLength;

        public string TestType { get; set; }
        public List<ITestCase> Tests { get; set; }

        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroup(JObject source) : this(source.ToObject<ExpandoObject>()) { }

        public TestGroup(dynamic source)
        {
            var expandoSource = (ExpandoObject) source;
            Cipher = EnumHelpers.GetEnumFromEnumDescription<Cipher>(expandoSource.GetTypeFromProperty<string>("cipher"));
            HashAlg = ShaAttributes.GetHashFunctionFromName(expandoSource.GetTypeFromProperty<string>("hashAlg"));

            Tests = new List<ITestCase>();
            foreach (var test in source.tests)
            {
                Tests.Add(new TestCase(test));
            }
        }

        public bool MergeTests(List<ITestCase> testsToMerge)
        {
            foreach (var test in Tests)
            {
                var matchingTest = testsToMerge.FirstOrDefault(t => t.TestCaseId == test.TestCaseId);
                if (matchingTest == null)
                {
                    return false;
                }
                if (!test.Merge(matchingTest))
                {
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            return
                $"{HashAlg.Name}|{Cipher}"
                    .GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var otherGroup = obj as TestGroup;
            if (otherGroup == null)
            {
                return false;
            }
            return this.GetHashCode() == otherGroup.GetHashCode();
        }

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
