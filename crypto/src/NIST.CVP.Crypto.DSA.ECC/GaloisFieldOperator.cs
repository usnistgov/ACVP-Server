using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.DSA.ECC
{
    public class GaloisFieldOperator : IFieldOperator
    {
        private readonly BigInteger _m;

        public GaloisFieldOperator(BigInteger modulo)
        {
            _m = modulo;
        }

        public BigInteger Add(BigInteger a, BigInteger b)
        {
            // Pulled from CAVS, EccMp.cpp, line 183
            // EccMPPoly::operator+
            return Modulo(a ^ b);
        }

        public BigInteger Multiply(BigInteger a, BigInteger b)
        {
            // Pulled from CAVS, EccMp.cpp, line 222
            // EccMPPoly::operator*

            BigInteger shorter, longer;
            if (a.ExactBitLength() < b.ExactBitLength())
            {
                shorter = a;
                longer = b;
            }
            else
            {
                shorter = b;
                longer = a;
            }

            var shortLen = shorter.ExactBitLength();
            var longLen = longer.ExactBitLength();
            BigInteger c, mask;

            for (var i = 0; i < shortLen; i++)
            {
                if (!shorter.GetBit(i))
                {
                    continue;
                }

                for (var j = 0; j < longLen; j++)
                {
                    mask = Convert.ToInt32(longer.GetBit(j));
                    mask <<= (i + j);
                    c ^= mask;
                }
            }

            return Modulo(c);
        }

        public BigInteger Divide(BigInteger a, BigInteger b)
        {
            // Pulled from CAVS, EccMp.cpp, line 305
            // EccMPPoly::DivMod

            var aLen = a.ExactBitLength();
            var bLen = b.ExactBitLength();

            // If a has less bits than the divisor, no division necessary
            if (aLen < bLen)
            {
                return 0;
            }

            b <<= ((aLen - bLen) + 1);
            var quotientBs = new BitString(aLen - bLen + 1);

            for (var i = aLen; i >= bLen; i--)
            {
                b >>= 1;
                if (a.GetBit(i - 1))
                {
                    // Set Quotient bit
                    quotientBs.Set(i - bLen, true);
                    a ^= b;
                }
                else
                {
                    // Set Quotient bit
                    quotientBs.Set(i - bLen, false);
                }
            }

            return quotientBs.ToPositiveBigInteger();
        }

        public BigInteger Inverse(BigInteger a)
        {
            // Pulled from CAVS, EccMp.cpp, line 399
            // EccMPPoly::Inverse
            BigInteger p1, p2, r1, r2, q1, q2, p, r, q;

            r2 = _m;
            r1 = a;

            p2 = 0;
            p1 = 1;

            q2 = 1;
            q1 = 0;

            while(true)
            {
                q = Divide(r2, r1);
                r = Modulo(r2, r1);
                p = Add(Multiply(q, p1), p2);

                if (r != 0)
                {
                    r2 = r1;
                    r1 = r;
                    q2 = q1;
                    q1 = q;
                    p2 = p1;
                    p1 = p;
                }
                else
                {
                    break;
                }
            }

            return p1;
        }

        public BigInteger Modulo(BigInteger a)
        {
            return Modulo(a, _m);
        }

        public BigInteger Modulo(BigInteger a, BigInteger m)
        {
            // Pulled from CAVS, EccMp.cpp, line 265
            // EccMPPoly::operator%

            // Need temporary variable
            var modulo = m;

            var aLen = a.ExactBitLength();
            var mLen = m.ExactBitLength();

            // If a has less bits than the modulo, no reduction necessary
            if (aLen < mLen)
            {
                return a;
            }

            modulo <<= ((aLen - mLen) + 1);

            for (var i = aLen; i >= mLen; i--)
            {
                modulo >>= 1;
                if (a.GetBit(i - 1))
                {
                    a ^= modulo;
                }
            }

            return a;
        }
    }
}
