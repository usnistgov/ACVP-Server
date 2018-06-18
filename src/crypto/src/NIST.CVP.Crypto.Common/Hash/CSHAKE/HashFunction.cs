namespace NIST.CVP.Crypto.Common.Hash.CSHAKE
{
    public struct HashFunction
    {
        public int DigestSize { get; set; }
        public int Capacity { get; set; }
        public bool XOF { get; set; }
        public string FunctionName { get; set; }
        public string Customization { get; set; }
    }
}
