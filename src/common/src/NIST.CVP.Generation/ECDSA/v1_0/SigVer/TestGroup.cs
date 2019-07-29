using Newtonsoft.Json;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;

namespace NIST.CVP.Generation.ECDSA.v1_0.SigVer
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; }
        [JsonProperty(PropertyName = "curve")]
        public Curve Curve { get; set; }

        [JsonIgnore]
        public bool IsMessageRandomized => "SP800-106".Equals(Conformance, StringComparison.OrdinalIgnoreCase);
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Conformance { get; set; } = string.Empty;

        [JsonIgnore] public HashFunction HashAlg { get; set; }
        [JsonProperty(PropertyName = "hashAlg")]
        public string HashAlgName
        {
            get => HashAlg?.Name;
            set => HashAlg = ShaAttributes.GetHashFunctionFromName(value);
        }

        [JsonIgnore] public ITestCaseExpectationProvider<EcdsaSignatureDisposition> TestCaseExpectationProvider { get; set; }

        public List<TestCase> Tests { get; set; } = new List<TestCase>();

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "curve":
                    Curve = EnumHelpers.GetEnumFromEnumDescription<Curve>(value);
                    return true;

                case "hashalg":
                    HashAlg = ShaAttributes.GetHashFunctionFromName(value);
                    return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return $"{EnumHelpers.GetEnumDescriptionFromEnum(Curve)}{HashAlg.Name}{IsMessageRandomized}".GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is TestGroup otherGroup)
            {
                return GetHashCode() == otherGroup.GetHashCode();
            }

            return false;
        }
    }
}
