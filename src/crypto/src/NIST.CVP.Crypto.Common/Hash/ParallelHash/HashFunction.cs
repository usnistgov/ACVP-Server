namespace NIST.CVP.Crypto.Common.Hash.ParallelHash
{
    public struct HashFunction
    {
        public HashFunction(int digestLength, int capacity, bool xof) : this()
        {
            DigestLength = digestLength;
            Capacity = capacity;
            XOF = xof;
        }

        public int DigestLength { get; set; }
        public int Capacity { get; set; }
        public bool XOF { get; set; }
    }
}
