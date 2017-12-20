using System.Dynamic;
using System.Numerics;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme.Ffc;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Crypto.SHAWrapper.Helpers;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class TestGroup : TestGroupBase<KasDsaAlgoAttributesFfc>
    {
        public TestGroup()
        {
            
        }

        public TestGroup(JObject source) : this(source.ToObject<ExpandoObject>()) { }

        public TestGroup(dynamic source)
        {
            MapToProperties(source);
        }

        public FfcScheme Scheme { get; set; }
        public FfcParameterSet ParmSet { get; set; }

        public BigInteger P { get; set; }
        public BigInteger Q { get; set; }
        public BigInteger G { get; set; }

        public override KasDsaAlgoAttributesFfc KasDsaAlgoAttributes => 
            new KasDsaAlgoAttributesFfc(Scheme, ParmSet);

        public override int GetHashCode()
        {
            return (
                $"{Scheme.ToString()}|{TestType}|{KasRole}|{KasMode}|{HashAlg.Name}|{MacType}|{KeyLen}|{AesCcmNonceLen}|{MacLen}|{KdfType}|{IdServerLen}|{IdServer}|{IdIutLen}|{IdIut}|{OiPattern}|{KcRole.ToString()}|{KcType.ToString()}|{NonceType}|{ParmSet}|{P}|{Q}|{G}"
            ).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TestGroupBase<KasDsaAlgoAttributesFfc> otherGroup))
            {
                return false;
            }
            return this.GetHashCode() == otherGroup.GetHashCode();
        }

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

        private void MapToProperties(dynamic source)
        {
            ExpandoObject expandoSource = (ExpandoObject)source;

            //Function = SpecificationMapping.FunctionArrayToFlags(source.function);
            Scheme = EnumHelpers.GetEnumFromEnumDescription<FfcScheme>(expandoSource.GetTypeFromProperty<string>("scheme"));
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

            P = expandoSource.GetBigIntegerFromProperty("p");
            Q = expandoSource.GetBigIntegerFromProperty("q");
            G = expandoSource.GetBigIntegerFromProperty("g");

            foreach (var test in source.tests)
            {
                Tests.Add(new TestCase(test));
            }
        }
    }
}