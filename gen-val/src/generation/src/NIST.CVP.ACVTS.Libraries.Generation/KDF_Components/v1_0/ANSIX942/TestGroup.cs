using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.AnsiX942.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.ANSIX942
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; }

        [JsonIgnore] public HashFunction HashAlg { get; set; }
        [JsonProperty(PropertyName = "hashAlg")]
        public string HashAlgName
        {
            get => HashAlg?.Name;
            set => HashAlg = ShaAttributes.GetHashFunctionFromName(value);
        }

        public AnsiX942Types KdfType { get; set; }
        public BitString Oid { get; set; } = new BitString("06 0b 2a 86 48 86 f7 0d 01 09 10 03 06");    // This is bad, hopefully temporary

        [JsonIgnore]
        public MathDomain KeyLen { get; set; }

        [JsonIgnore]
        public MathDomain OtherInfoLen { get; set; }

        [JsonIgnore]
        public MathDomain SuppInfoLen { get; set; }

        [JsonIgnore]
        public MathDomain ZzLen { get; set; }

        public List<TestCase> Tests { get; set; } = new List<TestCase>();
    }
}
