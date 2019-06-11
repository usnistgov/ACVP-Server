using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public NumeralString(int[] numbers)
        {
            Numbers = numbers;
        }

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
                numbers.Add(Int32.Parse(numberCandidate));
            }

            Numbers = numbers.ToArray();
        }

        public static BitString ToBitString(NumeralString numeralString)
        {
            var bs = new BitString(0);

            foreach (var number in numeralString.Numbers)
            {
                bs = bs.ConcatenateBits(BitString.To32BitString(number));
            }
            
            return bs;
        }

        public static NumeralString ToNumeralString(BitString bitString)
        {
            const int requiredMod = 32;
            
            if (bitString.BitLength % requiredMod != 0)
            {
                throw new ArgumentException($"Invalid modulus for {nameof(bitString)}");
            }

            var numbers = new List<int>();
            
            var num = new int[1];
            for (var i = 0; i <= bitString.BitLength / requiredMod; i++)
            {
                bitString.Substring(0 * i, requiredMod).Bits.CopyTo(num, 0);
                numbers.Add(num[0]);
            }
            
            return new NumeralString(numbers.ToArray());
        }
    }
}