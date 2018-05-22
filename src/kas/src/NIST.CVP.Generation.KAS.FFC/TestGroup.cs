using System.Dynamic;
using System.Numerics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS.FFC
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