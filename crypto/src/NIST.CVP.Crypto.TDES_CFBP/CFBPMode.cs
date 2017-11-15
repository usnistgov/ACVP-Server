using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;
using System;

namespace NIST.CVP.Crypto.TDES_CFBP
{
    public class CFBPMode
    {
        //Input: P1, P2, …, Pn; IV1, IV2, IV3. |Pi| = k, |IVj| = 64.
        public EncryptionResult BlockEncrypt(BitString key, BitString iv, BitString plainText)
        {






            //1. For i = 1, 2, 3, do
            for (var i = 1; i <= 3; i++)
            {
                //    a. Ii-1 = IVi;
                //    b. Oi = EK3(DK2(EK1(Ii-1)));
                //    c. Ci = Pi XOR {Oi}k;
                //    d. Output and feedback Ci.
            }

            //2. For i = 4, 5, …, n, do
            //    a. Ii-1 = Sk(Ii-2 |Ci-3);
            //    b. Oi = EK3(DK2(EK1(Ii-1)));
            //    c. Ci = Pi XOR {Oi}k;
            //    d. Output and feedback Ci.


            //Output: C1, C2, …, Cn; |Ci| = k.
            return null;
        }
    }
}
