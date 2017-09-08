using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Crypto.DSA.FFC
{
    public class Counter
    {
        private readonly int Count;
        private readonly int PCount;
        private readonly int QCount;

        public Counter(int count)
        {
            Count = count;
        }

        public Counter(int pCount, int qCount)
        {
            PCount = pCount;
            QCount = qCount;
        }
    }
}
