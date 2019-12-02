using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric.Helpers
{
    public static class XtsHelper
    {
        public static BitString GetIFromInteger(int dataUnitSeqNumber)
        {
            if (dataUnitSeqNumber < 0 || dataUnitSeqNumber > 255)
            {
                throw new ArgumentException("Invalid dataUnitSeqNumber in XTS");
            }

            var bsBytes = new byte[16];

            for (var i = 0; i < 16; i++)
            {
                bsBytes[i] = 0;
            }

            bsBytes[0] = Convert.ToByte(dataUnitSeqNumber);

            return new BitString(bsBytes);
        }
    }
}
