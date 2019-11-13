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
        
        /// <summary>
        /// Takes the modulo of a value (or <see cref="int"/> expression) and ensures it is between [0, <paramref name="modulo"/> - 1]
        /// </summary>
        /// <param name="value"></param>
        /// <param name="modulo"></param>
        /// <returns></returns>
        public static int PosMod(this int value, int modulo)
        {
            return (value % modulo + modulo) % modulo;
        }

        /// <summary>
        /// Gets the next number meeting the modulo (or the number itself if evenly divisible).
        /// </summary>
        /// <param name="value">The number to return or add to.</param>
        /// <param name="modulo">The desired modulo.</param>
        /// <returns>The value, or value + remainder of the value mod modulo.</returns>
        public static int ValueToMod(this int value, int modulo)
        {
            var remainder = value.PosMod(modulo); 
            if (remainder == 0)
            {
                return value;
            }

            return value + modulo - remainder;
        }
    }
}
