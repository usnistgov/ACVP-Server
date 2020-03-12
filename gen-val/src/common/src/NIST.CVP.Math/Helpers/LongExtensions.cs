namespace NIST.CVP.Math.Helpers
{
    public static class LongExtensions
    {
        public static long CeilingDivide(this long a, long b)
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