namespace NIST.CVP.Crypto.AES
{
    public struct Key
    {
        public DirectionValues Direction { get; set; }
        public byte[] Bytes { get; set; }
        public int BlockLength { get; set; }
        public int Length { get { return Bytes.Length; } }
        public RijndaelKeySchedule KeySchedule { get; set; }
    }
}
