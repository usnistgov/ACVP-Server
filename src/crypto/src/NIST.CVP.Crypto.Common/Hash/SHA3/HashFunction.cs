namespace NIST.CVP.Crypto.Common.Hash.SHA3
{
    public struct HashFunction
    {
        public HashFunction(int digestSize, int capacity, bool xof) : this()
        {
            DigestSize = digestSize;
            Capacity = capacity;
            XOF = xof;
        }

        public int DigestSize { get; set; }
        public int Capacity { get; set; }
        public bool XOF { get; set; }
    }
}
