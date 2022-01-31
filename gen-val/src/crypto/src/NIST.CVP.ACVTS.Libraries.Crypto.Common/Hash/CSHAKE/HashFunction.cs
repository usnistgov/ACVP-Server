namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.CSHAKE
{
    public struct HashFunction
    {
        public HashFunction(int digestLength, int capacity) : this()
        {
            DigestLength = digestLength;
            Capacity = capacity;
        }

        public int DigestLength { get; set; }
        public int Capacity { get; set; }
    }
}
