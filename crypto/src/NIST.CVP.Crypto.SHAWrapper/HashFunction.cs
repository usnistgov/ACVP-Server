namespace NIST.CVP.Crypto.SHAWrapper
{
    public class HashFunction
    {
        public ModeValues Mode { get; set; }
        public DigestSizes DigestSize { get; set; }
    }

    public enum ModeValues
    {
        SHA1,
        SHA2,
        SHA3
    }

    public enum DigestSizes
    {
        d160,
        d224,
        d256,
        d384,
        d512,
        d512t224,
        d512t256,
        NONE
    }
}