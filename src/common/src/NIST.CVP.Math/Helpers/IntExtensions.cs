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

        public static int IncrementOrReset(this int a, int min, int max, int increment = 1)
        {
            a++;
            if (a > max)
            {
                a -= max - min;
            }

            return a;
        }
    }
}
