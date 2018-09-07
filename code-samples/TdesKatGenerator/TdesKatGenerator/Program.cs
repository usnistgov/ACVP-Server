using CsvHelper;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Math;
using System.Collections.Generic;
using System.IO;

namespace TdesKatGenerator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var original = new AlgoArrayResponse
            {
                PlainText = new BitString("AB"),
                CipherText = new BitString("CD"),
                IV = new BitString("00"),
                Key1 = BitString.Ones(64),
                Key2 = BitString.Zeroes(64),
                Key3 = BitString.Ones(64)
            };

            var kats = new List<AlgoArrayResponse>();

            var writer = new StreamWriter(@"C:\Users\ctc\Documents\file.csv");
            var csv = new CsvWriter(writer);
            csv.Configuration.TypeConverterCache.AddConverter<BitString>(new BitStringConverter());

            var flippedKeys = FlipEachBit(original.Keys);
            flippedKeys.ForEach(fe =>
            {
                kats.Add(new AlgoArrayResponse
                {
                    Keys = fe,
                    PlainText = original.PlainText,
                    CipherText = original.CipherText,
                    IV = original.IV
                });
            });

            csv.WriteRecords(kats);
            writer.Close();
        }

        private static List<BitString> FlipEachBit(BitString original)
        {
            var list = new List<BitString>
            {
                original
            };

            for (var i = 0; i < original.BitLength; i++)
            {
                var flipped = original.GetDeepCopy();
                flipped.Set(i, !original.Bits[i]);

                list.Add(flipped);
            }

            return list;
        }
    }
}
