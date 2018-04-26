using NIST.CVP.Math.Helpers;
using System.Collections.Generic;

namespace NIST.CVP.Crypto.TDES
{
    public class TDESIVs
    {

        public byte[] R1 = { 0x55, 0x55, 0x55, 0x55, 0x55, 0x55, 0x55, 0x55 };
        public byte[] R2 = {0xaa, 0xaa, 0xaa, 0xaa, 0xaa, 0xaa, 0xaa, 0xaa};

        public List<byte[]> IVs { get; private set; }
        public TDESIVs(byte[] iv1)
        {
            IVs = new List<byte[]>();
            IVs.Add(iv1);
            var iv2 = iv1.Add(R1);
            IVs.Add(iv2);
            var iv3 = iv1.Add(R2);
            IVs.Add(iv3);
        }
    }
}
