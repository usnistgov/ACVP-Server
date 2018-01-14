using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.ANSIX963
{
    public class TestGroup : ITestGroup
    {
        public HashFunction HashAlg { get; set; }
        public int SharedInfoLength { get; set; }
        public int KeyDataLength { get; set; }
        public int FieldSize { get; set; }
        
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
            HashAlg = ShaAttributes.GetHashFunctionFromName(expandoSource.GetTypeFromProperty<string>("hashAlg"));
            SharedInfoLength = expandoSource.GetTypeFromProperty<int>("sharedInfoLength");
            KeyDataLength = expandoSource.GetTypeFromProperty<int>("keyDataLength");
            FieldSize = expandoSource.GetTypeFromProperty<int>("fieldSize");

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
                $"{HashAlg.Name}|{SharedInfoLength}|{KeyDataLength}|{FieldSize}"
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

                case "shared secret length":
                    FieldSize = int.Parse(value);
                    return true;

                case "sharedinfo length":
                    SharedInfoLength = int.Parse(value);
                    return true;

                case "key data length":
                    KeyDataLength = int.Parse(value);
                    return true;
            }

            return false;
        }
    }
}
