using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Symmetric.KeyWrap.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KeyWrap
{
    public abstract class TestGroupBase<TTestCase> : ITestGroup
        where TTestCase : TestCaseBase, new()
    {
        protected TestGroupBase()
        {
            Tests = new List<ITestCase>();
        }

        protected TestGroupBase(dynamic source)
        {
            LoadSource(source);
        }

        public int TestGroupId { get; set; }
        [JsonProperty(PropertyName = "testType")]
        public string TestType { get; set; } = "AFT";

        [JsonProperty(PropertyName = "direction")]
        public string Direction { get; set; }

        [JsonProperty(PropertyName = "kwCipher")]
        public string KwCipher { get; set; }

        [JsonProperty(PropertyName = "ptLen")]
        public int PtLen { get; set; }

        public abstract KeyWrapType KeyWrapType { get; set; }


        [JsonProperty(PropertyName = "keyLen")]
        public abstract int KeyLength { get; set; }

        public bool UseInverseCipher
        {
            get
            {
                if (KwCipher.Equals("inverse", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                return false;
            }
        }

        public List<ITestCase> Tests { get; set; }

        protected abstract void LoadSource(dynamic source);

        public abstract bool SetString(string name, string value);

    }
}
