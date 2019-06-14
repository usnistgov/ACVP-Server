using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NIST.CVP.Math
{
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
        /// Constructs a <see cref="NumeralString"/> using a provided array of int.
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
        /// <exception cref="ArgumentOutOfRangeException">Thrown when a number within the string is unable to be parsed into a int.</exception>
        public NumeralString(string baseTenNumbersSeparatedBySpace)
        {
            Regex regex = new Regex(@"^[\d ]*$");
            const int maxValue = 65535;

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
                    if (result > maxValue)
                    {
                        throw new ArgumentOutOfRangeException($"Provided {nameof(numberCandidate)} of {numberCandidate} exceeds maximum allowed value of {maxValue}.");
                    }

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
        /// Numerals within the <see cref="NumeralString"/> are int (16 bits) and are converted
        /// to bits and concatenated onto a <see cref="BitString"/> then returned.
        /// </summary>
        /// <param name="numeralString">The <see cref="NumeralString"/> to convert.</param>
        /// <returns>The <see cref="BitString"/> representation of a <see cref="NumeralString"/>.</returns>
        public static BitString ToBitString(NumeralString numeralString)
        {
            var bs = new BitString(0);

            foreach (var number in numeralString.Numbers)
            {
                bs = bs.ConcatenateBits(BitString.To32BitString(number).GetLeastSignificantBits(16));
            }

            return bs;
        }

        /// <summary>
        /// Converts a <see cref="BitString"/> to a <see cref="NumeralString"/>.
        ///
        /// <see cref="BitString"/> is expected to be a modulus of 8.
        /// </summary>
        /// <param name="bitString">The <see cref="BitString"/> to convert.</param>
        /// <returns><see cref="NumeralString"/> conversion of provided <see cref="BitString"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when the <see cref="BitString"/>'s BitLength is not a modulus of 8.</exception>
        public static NumeralString ToNumeralString(BitString bitString)
        {
            const int requiredMod = 16;

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