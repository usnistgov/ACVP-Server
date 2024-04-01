namespace NIST.CVP.ACVTS.Libraries.Math.Helpers
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
        
        /// <summary>
        /// Takes the modulo of a value (or <see cref="long"/> expression) and ensures it is between [0, <paramref name="modulo"/> - 1]
        /// </summary>
        /// <param name="value"></param>
        /// <param name="modulo"></param>
        /// <returns></returns>
        public static long PosMod(this long value, long modulo)
        {
            return (value % modulo + modulo) % modulo;
        }
        
        /// <summary>
        /// Takes the modulo of a value (or <see cref="int"/> expression) and ensures it is between (- <paramref name="modulo"/> / 2, <paramref name="modulo"/> / 2]
        /// </summary>
        /// <param name="value"></param>
        /// <param name="modulo"></param>
        /// <returns></returns>
        public static long PlusMinusMod(this long value, long modulo)
        {
            var reducedInput = value.PosMod(modulo);
            if (reducedInput <= (modulo / 2))
            {
                return reducedInput;
            }
            else
            {
                return reducedInput - modulo;
            }
        }
    }
}
