namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class AeadParameters
    {
        public int KeyLength { get; set; }
        public int IvLength { get; set; }
        public int PayloadLength { get; set; }
        public int SaltLength { get; set; }
        public int AadLength { get; set; }
        public int TagLength { get; set; }

        public bool ExternalSalt { get; set; }
        public bool ExternalIv { get; set; }

        public bool CouldFail { get; set; }
    }
}
