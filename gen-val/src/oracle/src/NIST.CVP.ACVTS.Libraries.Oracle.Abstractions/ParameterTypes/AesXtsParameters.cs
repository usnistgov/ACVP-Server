namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class AesXtsParameters : AesParameters
    {
        public string TweakMode { get; set; }
        public int DataUnitLength { get; set; }
    }
}
