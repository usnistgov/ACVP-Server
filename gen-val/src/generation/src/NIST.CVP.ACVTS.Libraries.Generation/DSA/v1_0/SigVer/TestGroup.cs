using System;
using System.Collections.Generic;
using System.Numerics;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.SigVer
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; }
        public int L { get; set; }
        public int N { get; set; }

        [JsonIgnore]
        public bool IsMessageRandomized => "SP800-106".Equals(Conformance, StringComparison.OrdinalIgnoreCase);
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Conformance { get; set; } = string.Empty;

        [JsonIgnore] public FfcDomainParameters DomainParams { get; set; } = new FfcDomainParameters();

        [JsonProperty(PropertyName = "p", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString P
        {
            get => DomainParams?.P != 0 ? new BitString(DomainParams.P, L) : null;
            set => DomainParams.P = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "q", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString Q
        {
            get => DomainParams?.Q != 0 ? new BitString(DomainParams.Q, N) : null;
            set => DomainParams.Q = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "g", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString G
        {
            get => DomainParams?.G != 0 ? new BitString(DomainParams.G, L) : null;
            set => DomainParams.G = value.ToPositiveBigInteger();
        }

        [JsonIgnore] public HashFunction HashAlg { get; set; }
        [JsonProperty(PropertyName = "hashAlg")]
        public string HashAlgName
        {
            get => HashAlg?.Name;
            set => HashAlg = ShaAttributes.GetHashFunctionFromName(value);
        }

        public List<TestCase> Tests { get; set; } = new List<TestCase>();

        [JsonIgnore]
        public ITestCaseExpectationProvider<DsaSignatureDisposition> TestCaseExpectationProvider { get; set; }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "p":
                    DomainParams.P = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "q":
                    DomainParams.Q = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "g":
                    DomainParams.G = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "l":
                    L = int.Parse(value);
                    return true;

                case "n":
                    N = int.Parse(value);
                    return true;

                case "hashalg":
                    HashAlgName = value;
                    return true;
            }

            return false;
        }
    }
}
