namespace NIST.CVP.ACVTS.Libraries.Generation.Core
{
    public class ParametersBase : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; }
    }
}
