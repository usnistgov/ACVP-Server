namespace NIST.CVP.Crypto.Common.Hash.SHA2
{
    public class HashFunction
    {
        public ModeValues Mode { get; set; }
        public DigestSizes DigestSize { get; set; }

        public HashFunction() { }

        public HashFunction(ModeValues mode, DigestSizes digestSize)
        {
            Mode = mode;
            DigestSize = digestSize;
        }
    }
}
