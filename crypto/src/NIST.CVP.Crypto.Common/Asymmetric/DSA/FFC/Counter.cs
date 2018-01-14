using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC
{
    public class Counter
    {
        public int Count { get; private set; }
        public int PCount { get; private set; }
        public int QCount { get; private set; }
        public PrimeGenMode Mode { get; private set; }

        public Counter(int count)
        {
            Count = count;
            Mode = PrimeGenMode.Probable;
        }

        public Counter(int pCount, int qCount)
        {
            PCount = pCount;
            QCount = qCount;
            Mode = PrimeGenMode.Provable;
        }

        public int GetCounter()
        {
            return Count;
        }
    }
}
