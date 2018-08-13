namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class KmacParameters
    {
        public int KeyLength { get; set; }
        public int MessageLength { get; set; }
        public int MacLength { get; set; }
        public bool HexCustomization { get; set; }
        public int CustomizationLength { get; set; }
        public int DigestSize { get; set; }
        public bool XOF { get; set; }
        public bool CouldFail { get; set; }
    }
}
