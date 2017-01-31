using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SHA.SHAProperties
{
    public class SHA160Properties : SHAPropertiesBase
    {
        public override int AppendedLength { get { return 64; } }
        public override int BlockSize { get { return 512; } }
        public override int DigestBitSize { get { return 160; } }
        public override int Rounds { get { return 80; } }
        public override int WordSize { get { return 32; } }
        public override int[,] SumShifts { get { return new int[,] { { 2, 13, 22 }, { 6, 11, 25 } }; } }
        public override int[,] SigmaShifts { get { return new int[,] { { 7, 18, 3 }, { 17, 19, 10 } }; } }

        public override BitString[] HValues
        {
            get
            {
                return PrepareValues(new string[] { "67452301", "EFCDAB89", "98BADCFE", "10325476", "C3D2E1F0" });
            }
        }

        public override BitString[] KValues
        {
            get
            {
                return PrepareValues(new string[] { "5A827999", "6ED9EBA1", "8F1BBCDC", "CA62C1D6" });
            }
        }
    }
}
