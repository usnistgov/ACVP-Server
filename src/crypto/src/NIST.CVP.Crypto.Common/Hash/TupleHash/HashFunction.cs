namespace NIST.CVP.Crypto.Common.Hash.TupleHash
{
    public struct HashFunction
    {
        public int DigestLength { get; set; }
        public int Capacity { get; set; }
        public bool XOF { get; set; }
        public string Customization { get; set; }
    }
}
