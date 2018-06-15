namespace NIST.CVP.Crypto.Common.Hash.TupleHash
{
    public struct HashFunction
    {
        public int DigestSize { get; set; }
        public int Capacity { get; set; }
        public bool XOF { get; set; }
    }
}
