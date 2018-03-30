namespace NIST.CVP.Crypto.Common.Hash.SHA2.SHAProperties
{
    public abstract class SHAPropertiesBase
    {
        public abstract int BlockSize { get; }
        public abstract int AppendedLength { get; }
        public abstract int DigestBitSize { get; }
        public abstract int Rounds { get; }
        public abstract int WordSize { get; }
        public abstract int[,] SumShifts { get; }
        public abstract int[,] SigmaShifts { get; }
        public abstract string[] KValues { get; }
        public abstract string[] HValues { get; }
    }
}
