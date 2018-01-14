using System.Dynamic;
using Newtonsoft.Json.Linq;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme.Ecc;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.KAS.ECC
{
    public class TestGroup : TestGroupBase<KasDsaAlgoAttributesEcc>
    {
        public TestGroup()
        {

        }

        public TestGroup(JObject source) : this(source.ToObject<ExpandoObject>()) { }

        public TestGroup(dynamic source)
        {
            MapToProperties(source);
        }

        public EccScheme Scheme { get; set; }
        public EccParameterSet ParmSet { get; set; }
        public Curve CurveName { get; set; }
        
        public override KasDsaAlgoAttributesEcc KasDsaAlgoAttributes =>
            new KasDsaAlgoAttributesEcc(Scheme, ParmSet, CurveName);

        public override int GetHashCode()
        {
            return (
                $"{Scheme.ToString()}|{TestType}|{KasRole}|{KasMode}|{HashAlg.Name}|{MacType}|{KeyLen}|{AesCcmNonceLen}|{MacLen}|{KdfType}|{IdServerLen}|{IdServer}|{IdIutLen}|{IdIut}|{OiPattern}|{KcRole.ToString()}|{KcType.ToString()}|{NonceType}|{ParmSet}|{CurveName}"
            ).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TestGroupBase<KasDsaAlgoAttributesEcc> otherGroup))
            {
                return false;
            }
            return this.GetHashCode() == otherGroup.GetHashCode();
        }

        private void MapToProperties(dynamic source)
        {
            ExpandoObject expandoSource = (ExpandoObject)source;

            //Function = SpecificationMapping.FunctionArrayToFlags(source.function);
            Scheme = EnumHelpers.GetEnumFromEnumDescription<EccScheme>(expandoSource.GetTypeFromProperty<string>("scheme"));

            TestType = expandoSource.GetTypeFromProperty<string>("testType");
            KasRole = EnumHelpers.GetEnumFromEnumDescription<KeyAgreementRole>(expandoSource.GetTypeFromProperty<string>("kasRole"));
            KasMode = EnumHelpers.GetEnumFromEnumDescription<KasMode>(expandoSource.GetTypeFromProperty<string>("kasMode"));

            var hashAttributes = ShaAttributes.GetShaAttributes(expandoSource.GetTypeFromProperty<string>("hashAlg"));
            HashAlg = new HashFunction(hashAttributes.mode, hashAttributes.digestSize);

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

            CurveName = EnumHelpers.GetEnumFromEnumDescription<Curve>(expandoSource.GetTypeFromProperty<string>("curveName"));

            foreach (var test in source.tests)
            {
                Tests.Add(new TestCase(test));
            }
        }
    }
}