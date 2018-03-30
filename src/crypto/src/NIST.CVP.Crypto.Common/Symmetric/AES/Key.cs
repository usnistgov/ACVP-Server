namespace NIST.CVP.Crypto.Common.Symmetric.AES
{
    public struct Key
    {
        public DirectionValues Direction { get; set; }
        public bool UseInverseCipher { get; set; }
        public byte[] Bytes { get; set; }
        public int BlockLength { get; set; }
        public int Length => Bytes.Length;
        public IRijndaelKeySchedule KeySchedule { get; set; }
    }
}
