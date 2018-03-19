using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace NIST.CVP.Generation.KeyWrap.TDES
{
    public class TestVectorSet : TestVectorSetBase<TestGroup, TestCase>
    {
        public override string Algorithm { get; set; } = "KeyWrap";
        public override string Mode { get; set; } = "TDES";
    }
}
