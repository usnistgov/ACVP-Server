using CsvHelper;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
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
                PlainText = BitString.Zeroes(64),
                IV = BitString.Zeroes(64),
                Key1 = new BitString("0101010101010101").ToOddParityBitString(),
                Key2 = new BitString("0202020202020202").ToOddParityBitString(),
                Key3 = new BitString("0303030303030303").ToOddParityBitString()
            };

            var kats = new List<AlgoArrayResponse>();
            var ecb = new EcbBlockCipher(new TdesEngine());

            var writer = new StreamWriter(@"C:\Users\ctc\Documents\variable-text.csv");
            var csv = new CsvWriter(writer);
            csv.Configuration.TypeConverterCache.AddConverter<BitString>(new BitStringConverter());

            var flipped = FlipEachBit(original.PlainText);
            flipped.ForEach(fe =>
            {
                var param = new ModeBlockCipherParameters(
                    BlockCipherDirections.Encrypt,
                    original.IV,
                    original.Keys,
                    fe
                );

                var result = ecb.ProcessPayload(param);

                kats.Add(new AlgoArrayResponse
                {
                    Keys = original.Keys,
                    PlainText = fe,
                    IV = original.IV,
                    CipherText = result.Result
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
