namespace NIST.CVP.Generation.Core.Tests.Fakes
{
    public class FakeParameters : IParameters
    {
        public int VectorSetId { get; set; } = 42;
        public string Algorithm { get; set; } = "test";
        public string Mode { get; set; } = "test2";
        public bool IsSample { get; set; } = true;
        public string[] Conformances { get; set; } = { };
    }
}
