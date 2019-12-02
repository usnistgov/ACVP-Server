using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KDF.Components.AnsiX942.Enums;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;

namespace NIST.CVP.Generation.KDF_Components.v1_0.ANSIX942
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; }
        public List<TestCase> Tests { get; set; } = new List<TestCase>();

        [JsonIgnore] public HashFunction HashAlg { get; set; }
        [JsonProperty(PropertyName = "hashAlg")]
        public string HashAlgName
        {
            get => HashAlg?.Name;
            set => HashAlg = ShaAttributes.GetHashFunctionFromName(value);
        }

        public int KeyLen { get; set; }
        public AnsiX942Types KdfType { get; set; }
        public int OtherInfoLen { get; set; }
        public int ZzLen { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is TestGroup group)
            {
                return GetHashCode() == group.GetHashCode();
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TestGroupId, TestType, HashAlgName, KeyLen, KdfType, OtherInfoLen, ZzLen);
        }
    }
}
