using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json.Linq;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Crypto.Common.KDF.Components.IKEv1.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.IKEv1
{
    public class TestGroup : ITestGroup
    {
        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroup(JObject source) : this(source.ToObject<ExpandoObject>()) { }

        public TestGroup(dynamic source)
        {
            var expandoSource = (ExpandoObject) source;
            TestGroupId = expandoSource.GetTypeFromProperty<int>("tgId");
            HashAlg = ShaAttributes.GetHashFunctionFromName(expandoSource.GetTypeFromProperty<string>("hashAlg"));
            AuthenticationMethod = EnumHelpers.GetEnumFromEnumDescription<AuthenticationMethods>(expandoSource.GetTypeFromProperty<string>("authenticationMethod"));
            GxyLength = expandoSource.GetTypeFromProperty<int>("dhLength");
            NInitLength = expandoSource.GetTypeFromProperty<int>("nInitLength");
            NRespLength = expandoSource.GetTypeFromProperty<int>("nRespLength");
            PreSharedKeyLength = expandoSource.GetTypeFromProperty<int>("preSharedKeyLength");

            Tests = new List<ITestCase>();
            foreach (var test in source.tests)
            {
                Tests.Add(new TestCase(test));
            }
        }

        public int TestGroupId { get; set; }
        public HashFunction HashAlg { get; set; }
        public AuthenticationMethods AuthenticationMethod { get; set; }
        public int GxyLength { get; set; }
        public int NInitLength { get; set; }
        public int NRespLength { get; set; }
        public int PreSharedKeyLength { get; set; }

        public string TestType { get; set; }
        public List<ITestCase> Tests { get; set; }

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

                case "g^xy length":
                    GxyLength = int.Parse(value);
                    return true;

                case "nr length":
                    NRespLength = int.Parse(value);
                    return true;

                case "hashalg":
                    HashAlg = ShaAttributes.GetHashFunctionFromName(value);
                    return true;
                
                case "pre-shared-key length":
                    PreSharedKeyLength = int.Parse(value);
                    return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return
                $"{HashAlg.Name}|{EnumHelpers.GetEnumDescriptionFromEnum(AuthenticationMethod)}|{GxyLength}|{NInitLength}|{NRespLength}|{PreSharedKeyLength}"
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
    }
}
