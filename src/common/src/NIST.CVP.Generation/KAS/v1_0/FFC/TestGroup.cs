using System.Numerics;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS.v1_0.FFC
{
    public class TestGroup : TestGroupBase<TestGroup, TestCase, KasDsaAlgoAttributesFfc>
    {
        public FfcScheme Scheme { get; set; }

        public FfcParameterSet ParmSet { get; set; }

        public int L { get; set; }
        public int N { get; set; }
        [JsonIgnore] public FfcDomainParameters DomainParams { get; set; } = new FfcDomainParameters();

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString P
        {
            get => DomainParams?.P != 0 ? new BitString(DomainParams.P, L) : null;
            set => DomainParams.P = value.ToPositiveBigInteger();
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString Q
        {
            get => DomainParams?.Q != 0 ? new BitString(DomainParams.Q, N) : null;
            set => DomainParams.Q = value.ToPositiveBigInteger();
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString G
        {
            get => DomainParams?.G != 0 ? new BitString(DomainParams.G, L) : null;
            set => DomainParams.G = value.ToPositiveBigInteger();
        }

        public override KasDsaAlgoAttributesFfc KasAlgoAttributes => 
            new KasDsaAlgoAttributesFfc(Scheme, ParmSet);
        
        public override SchemeKeyNonceGenRequirement KeyNonceGenRequirementsIut => 
            KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
                KasAlgoAttributes.Scheme, KasMode, KasRole, KcRole, KcType
            );

        public override SchemeKeyNonceGenRequirement KeyNonceGenRequirementsServer => 
            KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
                KasAlgoAttributes.Scheme, 
                KasMode, 
                KeyGenerationRequirementsHelper.GetOtherPartyKeyAgreementRole(KasRole), 
                KeyGenerationRequirementsHelper.GetOtherPartyKeyConfirmationRole(KcRole), 
                KcType
            );

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "p":
                    P = new BitString(value);
                    return true;
                case "q":
                    Q = new BitString(value);
                    return true;
                case "g":
                    G = new BitString(value);
                    return true;
            }
            return false;
        }
    }
}