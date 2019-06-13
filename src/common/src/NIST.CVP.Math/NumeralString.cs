using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NIST.CVP.Math
{
    /// <summary>
    /// Represents a series of short numbers.
    ///
    /// This class is used within AES-FFX.  The reason the "short" data type was chosen,
    /// and 16 bit numbers utilized is due to the fact that FFX works with the idea of an alphabet and radix,
    /// which cannot exceed a maximum set of 65536.  This means there are a maximum total of 65536 values that can be represented
    /// in any alphabet, or 2^16, allowing the max number to completely fill up a 16 bit block.
    /// </summary>
    public class NumeralString
    {
        public int[] Numbers { get; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var i in Numbers)
            {
                sb.Append(i);
                sb.Append(" ");
            }

            // Remove the final space
            sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }

        /// <summary>
        /// Constructs a <see cref="NumeralString"/> using a provided array of short.
        /// </summary>
        /// <param name="numbers">The numbers to make up the <see cref="NumeralString"/>.</param>
        public NumeralString(int[] numbers)
        {
            Numbers = numbers;
        }

        /// <summary>
        /// Constructs a NumeralString using a provided string of base ten numbers separated by spaces.
        /// </summary>
        /// <param name="baseTenNumbersSeparatedBySpace">The numbers that are to make up the numeral string.  Should be in base 10 and separated by a space.</param>
        /// <exception cref="ArgumentException">Thrown when provided string contains invalid characters.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when a number within the string is unable to be parsed into a short.</exception>
        public NumeralString(string baseTenNumbersSeparatedBySpace)
        {
            Regex regex = new Regex(@"^[\d ]*$");

            if (!regex.IsMatch(baseTenNumbersSeparatedBySpace))
            {
                throw new ArgumentException($"Invalid characters contained within nameof({baseTenNumbersSeparatedBySpace})");
            }

            var numberCandidates = baseTenNumbersSeparatedBySpace.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var numbers = new List<int>();
            foreach (var numberCandidate in numberCandidates)
            {
                if (int.TryParse(numberCandidate, out var result))
                {
                    numbers.Add(result);
                    continue;
                }

                throw new ArgumentOutOfRangeException($"{nameof(numberCandidate)} of {numberCandidate} could not be parsed into a {typeof(int)}.");
            }

            Numbers = numbers.ToArray();
        }

        /// <summary>
        /// Converts a provided <see cref="NumeralString"/> to a <see cref="BitString"/>.
        ///
        /// Numerals within the <see cref="NumeralString"/> are short (16 bits) and are converted
        /// to bits and concatenated onto a <see cref="BitString"/> then returned.
        /// </summary>
        /// <param name="numeralString">The <see cref="NumeralString"/> to convert.</param>
        /// <returns>The <see cref="BitString"/> representation of a <see cref="NumeralString"/>.</returns>
        public static BitString ToBitString(NumeralString numeralString)
        {
            var bs = new BitString(0);

            foreach (var number in numeralString.Numbers)
            {
                bs = bs.ConcatenateBits(BitString.To32BitString(number).GetLeastSignificantBits(8));
            }

            return bs;
        }

        /// <summary>
        /// Converts a <see cref="BitString"/> to a <see cref="NumeralString"/>.
        ///
        /// <see cref="BitString"/> is expected to be a modulus of 16, as the individual
        /// numerals within the <see cref="BitString"/> are expected to be <see cref="short"/>.
        /// </summary>
        /// <param name="bitString">The <see cref="BitString"/> to convert.</param>
        /// <returns><see cref="NumeralString"/> conversion of provided <see cref="BitString"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when the <see cref="BitString"/>'s BitLength is not a modulus of 16.</exception>
        public static NumeralString ToNumeralString(BitString bitString)
        {
            const int requiredMod = 8;

            if (bitString.BitLength % requiredMod != 0)
            {
                throw new ArgumentException($"Invalid modulus for {nameof(bitString)}");
            }

            var numbers = new List<int>();

            for (var i = 0; i < bitString.BitLength / requiredMod; i++)
            {
                var subString = bitString.Substring(bitString.BitLength - ((i + 1) * requiredMod), requiredMod).ToHex();
                numbers.Add(Convert.ToInt32($"0x{subString}", 16));
            }

            return new NumeralString(numbers.ToArray());
        }
    }
}