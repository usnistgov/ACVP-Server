namespace NIST.CVP.Generation.KeyWrap.v1_0.AES
{
    public class TestVectorSet : TestVectorSetBase<TestGroup, TestCase>
    {
        public override string Algorithm { get; set; } = "KeyWrap";
        public override string Mode { get; set; } = "AES";
    }
}
