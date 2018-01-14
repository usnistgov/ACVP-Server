using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.IKEv2
{
    public class TestGroup : ITestGroup
    {
        public HashFunction HashAlg { get; set; }
        public int GirLength { get; set; }
        public int NInitLength { get; set; }
        public int NRespLength { get; set; }
        public int DerivedKeyingMaterialLength { get; set; }
        
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
            GirLength = expandoSource.GetTypeFromProperty<int>("dhLength");
            NInitLength = expandoSource.GetTypeFromProperty<int>("nInitLength");
            NRespLength = expandoSource.GetTypeFromProperty<int>("nRespLength");
            DerivedKeyingMaterialLength = expandoSource.GetTypeFromProperty<int>("preSharedKeyLength");

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
                $"{HashAlg.Name}|{GirLength}|{NInitLength}|{NRespLength}|{DerivedKeyingMaterialLength}"
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
                case "nilength":
                case "ni length":
                    NInitLength = int.Parse(value);
                    return true;

                case "g^ir length":
                    GirLength = int.Parse(value);
                    return true;

                case "nr length":
                    NRespLength = int.Parse(value);
                    return true;

                case "hashalg":
                    HashAlg = ShaAttributes.GetHashFunctionFromName(value);
                    return true;
                
                case "dkm length":
                    DerivedKeyingMaterialLength = int.Parse(value);
                    return true;
            }

            return false;
        }
    }
}
