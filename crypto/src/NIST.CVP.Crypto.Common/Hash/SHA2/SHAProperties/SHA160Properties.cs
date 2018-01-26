namespace NIST.CVP.Crypto.Common.Hash.SHA2.SHAProperties
{
    public class SHA160Properties : SHAPropertiesBase
    {
        public override int AppendedLength => 64;
        public override int BlockSize => 512;
        public override int DigestBitSize => 160;
        public override int Rounds => 80;
        public override int WordSize => 32;
        public override int[,] SumShifts => new [,] { { 2, 13, 22 }, { 6, 11, 25 } };
        public override int[,] SigmaShifts => new [,] { { 7, 18, 3 }, { 17, 19, 10 } };
        public override string[] HValues => new [] {"67452301", "efcdab89", "98badcfe", "10325476", "c3d2e1f0"};
        public override string[] KValues => new [] {"5a827999", "6ed9eba1", "8f1bbcdc", "ca62c1d6"};
    }
}
