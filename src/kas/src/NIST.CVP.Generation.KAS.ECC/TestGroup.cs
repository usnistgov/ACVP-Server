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
        public Curve Curve { get; set; }
        
        public override KasDsaAlgoAttributesEcc KasDsaAlgoAttributes =>
            new KasDsaAlgoAttributesEcc(Scheme, ParmSet, Curve);

        
    }
}