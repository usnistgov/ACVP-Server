using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Math.Helpers
{
    public static class IntExtensions
    {
        public static int CeilingDivide(this int a, int b)
        {
            // Modulo is slow, avoid it
            var result = a / b;
            if (result * b != a)
            {
                result++;
            }

            return result;
        }
    }
}
