using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_KC
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; }
        public List<TestCase> Tests { get; set; } = new();
        public KeyAgreementRole KasRole { get; set; }
        public KeyConfirmationDirection KeyConfirmationDirection { get; set; }
        public KeyConfirmationRole KeyConfirmationRole { get; set; }
        public KeyAgreementMacType KeyAgreementMacType { get; set; }
        public int KeyLen { get; set; }
        public int MacLen { get; set; }
        public bool IsSample { get; set; }
    }
}
