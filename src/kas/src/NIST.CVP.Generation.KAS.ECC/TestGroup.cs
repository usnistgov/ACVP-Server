using System.Dynamic;
using Newtonsoft.Json.Linq;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.KAS.ECC
{
    public class TestGroup : TestGroupBase<TestGroup, TestCase, KasDsaAlgoAttributesEcc>
    {
        public EccScheme Scheme { get; set; }
        public EccParameterSet ParmSet { get; set; }
        public Curve CurveName { get; set; }
        
        public override KasDsaAlgoAttributesEcc KasDsaAlgoAttributes =>
            new KasDsaAlgoAttributesEcc(Scheme, ParmSet, CurveName);

        private void MapToProperties(dynamic source)
        {
            ExpandoObject expandoSource = (ExpandoObject)source;

            TestGroupId = (int) source.tgId;
            Scheme = EnumHelpers.GetEnumFromEnumDescription<EccScheme>(expandoSource.GetTypeFromProperty<string>("scheme"), false);

            TestType = expandoSource.GetTypeFromProperty<string>("testType");
            KasRole = EnumHelpers.GetEnumFromEnumDescription<KeyAgreementRole>(expandoSource.GetTypeFromProperty<string>("kasRole"), false);
            KasMode = EnumHelpers.GetEnumFromEnumDescription<KasMode>(expandoSource.GetTypeFromProperty<string>("kasMode"), false);

            var hashValue = expandoSource.GetTypeFromProperty<string>("hashAlg");
            if (!string.IsNullOrEmpty(hashValue))
            {
                HashAlg = ShaAttributes.GetHashFunctionFromName(hashValue);
            }

            MacType = EnumHelpers.GetEnumFromEnumDescription<KeyAgreementMacType>(expandoSource.GetTypeFromProperty<string>("macType"), false);

            KeyLen = expandoSource.GetTypeFromProperty<int>("keyLen");
            AesCcmNonceLen = expandoSource.GetTypeFromProperty<int>("aesCcmNonceLen");
            MacLen = expandoSource.GetTypeFromProperty<int>("macLen");
            KdfType = expandoSource.GetTypeFromProperty<string>("kdfType");
            IdServerLen = expandoSource.GetTypeFromProperty<int>("idServerLen");
            IdServer = expandoSource.GetBitStringFromProperty("idServer");
            IdIutLen = expandoSource.GetTypeFromProperty<int>("idIutLen");
            IdIut = expandoSource.GetBitStringFromProperty("idIut");
            OiPattern = expandoSource.GetTypeFromProperty<string>("oiPattern");
            KcRole = EnumHelpers.GetEnumFromEnumDescription<KeyConfirmationRole>(expandoSource.GetTypeFromProperty<string>("kcRole"), false);
            KcType = EnumHelpers.GetEnumFromEnumDescription<KeyConfirmationDirection>(expandoSource.GetTypeFromProperty<string>("kcType"), false);
            NonceType = expandoSource.GetTypeFromProperty<string>("nonceType");

            CurveName = EnumHelpers.GetEnumFromEnumDescription<Curve>(expandoSource.GetTypeFromProperty<string>("curveName"), false);
        }
    }
}