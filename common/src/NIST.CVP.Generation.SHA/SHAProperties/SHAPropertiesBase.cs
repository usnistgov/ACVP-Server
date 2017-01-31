using NIST.CVP.Math;
using System.Linq;

namespace NIST.CVP.Generation.SHA
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
        public abstract BitString[] KValues { get; }
        public abstract BitString[] HValues { get; }

        public BitString[] PrepareValues(string[] values)
        {
            var valuesBS = new BitString[values.Count()];

            for(var i = 0; i < values.Count(); i++)
            {
                valuesBS[i] = new BitString(values[i]);
            }

            return valuesBS;
        }
    }
}
