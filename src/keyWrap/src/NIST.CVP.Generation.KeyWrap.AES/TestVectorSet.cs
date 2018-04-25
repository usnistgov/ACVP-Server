namespace NIST.CVP.Generation.KeyWrap.AES
{
    public class TestVectorSet : TestVectorSetBase<TestGroup, TestCase>
    {
        public override string Algorithm { get; set; } = "KeyWrap";
        public override string Mode { get; set; } = "AES";
    }
}
