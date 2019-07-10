using System.Numerics;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS.v1_0.FFC
{
    public class TestGroup : TestGroupBase<TestGroup, TestCase, KasDsaAlgoAttributesFfc>
    {
        public FfcScheme Scheme { get; set; }

        public FfcParameterSet ParmSet { get; set; }


        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger P { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger Q { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger G { get; set; }

        public override KasDsaAlgoAttributesFfc KasDsaAlgoAttributes => 
            new KasDsaAlgoAttributesFfc(Scheme, ParmSet);
        
        public override SchemeKeyNonceGenRequirement KeyNonceGenRequirementsIut => 
            KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
                KasDsaAlgoAttributes.Scheme, KasMode, KasRole, KcRole, KcType
            );

        public override SchemeKeyNonceGenRequirement KeyNonceGenRequirementsServer => 
            KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
                KasDsaAlgoAttributes.Scheme, 
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
                    P = new BitString(value).ToPositiveBigInteger();
                    return true;
                case "q":
                    Q = new BitString(value).ToPositiveBigInteger();
                    return true;
                case "g":
                    G = new BitString(value).ToPositiveBigInteger();
                    return true;
            }
            return false;
        }
    }
}