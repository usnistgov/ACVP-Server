using System.Dynamic;
using System.Numerics;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Crypto.SHAWrapper.Helpers;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class TestGroup : TestGroupBase
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

        public override int GetHashCode()
        {
            return (
                $"{Scheme}|{TestType}|{KasRole}|{KasMode}|{ParmSet}|{HashAlg}|{MacType}|{KeyLen}|{NonceAesCcmLen}|{MacLen}|{KdfType}|{IdServerLen}|{IdServer}|{IdIutLen}|{IdIut}|{OiPattern}|{KcRole}|{KcType}"
            ).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TestGroupBase otherGroup))
            {
                return false;
            }
            return this.GetHashCode() == otherGroup.GetHashCode();
        }

        private void MapToProperties(dynamic source)
        {
            ExpandoObject expandoSource = (ExpandoObject)source;

            //Function = SpecificationMapping.FunctionArrayToFlags(source.function);
            Scheme = expandoSource.GetTypeFromProperty<FfcScheme>("scheme");
            TestType = expandoSource.GetTypeFromProperty<string>("testType");
            KasRole = expandoSource.GetTypeFromProperty<KeyAgreementRole>("kasRole");
            KasMode = expandoSource.GetTypeFromProperty<KasMode>("kasMode");

            var hashAttributes = ShaAttributes.GetShaAttributes(expandoSource.GetTypeFromProperty<string>("hashAlg"));
            HashAlg = new HashFunction(hashAttributes.mode, hashAttributes.digestSize);

            MacType = expandoSource.GetTypeFromProperty<KeyAgreementMacType>("macType");
            KeyLen = expandoSource.GetTypeFromProperty<int>("keyLen");
            NonceAesCcmLen = expandoSource.GetTypeFromProperty<int>("nonceAesCcmLen");
            MacLen = expandoSource.GetTypeFromProperty<int>("macLen");
            KdfType = expandoSource.GetTypeFromProperty<string>("kdfType");
            IdServerLen = expandoSource.GetTypeFromProperty<int>("idServerLen");
            IdServer = expandoSource.GetBitStringFromProperty("idServer");
            IdIutLen = expandoSource.GetTypeFromProperty<int>("idIutLen");
            IdIut = expandoSource.GetBitStringFromProperty("idIut");
            OiPattern = expandoSource.GetTypeFromProperty<string>("oiPattern");
            KcRole = expandoSource.GetTypeFromProperty<KeyConfirmationRole>("kcRole"); ;
            KcType = expandoSource.GetTypeFromProperty<KeyConfirmationDirection>("kcType");

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