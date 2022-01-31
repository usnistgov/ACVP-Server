using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC
{
    public class Counter
    {
        public int Count { get; set; }
        public int PCount { get; set; }
        public int QCount { get; set; }

        public PrimeGenMode Mode
        {
            get
            {
                if (PCount != 0 && QCount != 0)
                {
                    return PrimeGenMode.Provable;
                }

                return PrimeGenMode.Probable;
            }
        }

        public Counter()
        {

        }

        public Counter(int count)
        {
            Count = count;
        }

        public Counter(int pCount, int qCount)
        {
            PCount = pCount;
            QCount = qCount;
        }

        public int GetCounter()
        {
            return Count;
        }
    }
}
