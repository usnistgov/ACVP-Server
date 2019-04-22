using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS.v1_0
{
    public abstract class TestGroupBase<TTestGroup, TTestCase, TKasDsaAlgoAttributes> : ITestGroup<TTestGroup, TTestCase>
        where TKasDsaAlgoAttributes : IKasDsaAlgoAttributes
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase, TKasDsaAlgoAttributes>
        where TTestCase : TestCaseBase<TTestGroup, TTestCase, TKasDsaAlgoAttributes>
    {
        public int TestGroupId { get; set; }

        public KasAssurance Function { get; set; }

        public string TestType { get; set; }

        public KeyAgreementRole KasRole { get; set; }

        public KasMode KasMode { get; set; }

        [JsonIgnore] public HashFunction HashAlg { get; set; }
        [JsonProperty(PropertyName = "hashAlg")]
        public string HashAlgName
        {
            get => HashAlg?.Name;
            set => HashAlg = ShaAttributes.GetHashFunctionFromName(value);
        }

        public KeyAgreementMacType MacType { get; set; }

        public int KeyLen { get; set; }

        public int AesCcmNonceLen { get; set; }

        public int MacLen { get; set; }

        public string KdfType { get; set; }

        public int IdServerLen { get; set; } = 48;

        public BitString IdServer { get; set; } = new BitString("434156536964");

        public int IdIutLen { get; set; }

        public BitString IdIut { get; set; }

        public string OiPattern { get; set; }

        public KeyConfirmationRole KcRole { get; set; }

        public KeyConfirmationDirection KcType { get; set; }

        public string NonceType { get; set; }

        [JsonIgnore]
        public abstract TKasDsaAlgoAttributes KasDsaAlgoAttributes { get; }

        [JsonIgnore]
        public abstract SchemeKeyNonceGenRequirement KeyNonceGenRequirementsIut { get; }

        [JsonIgnore]
        public abstract SchemeKeyNonceGenRequirement KeyNonceGenRequirementsServer { get; }

        public List<TTestCase> Tests { get; set; } = new List<TTestCase>();
    }
}
