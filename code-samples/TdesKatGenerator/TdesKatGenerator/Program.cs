using CsvHelper;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Math;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace TdesKatGenerator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            GenerateInversePermutation();
            GenerateVariableKey();
            GenerateVariableText();
            GeneratePermutation();
            GenerateSubstitution();
        }
        
        private static void GeneratePermutation()
        {
            var kats = new List<AlgoArrayResponse>();
            var ecb = new EcbBlockCipher(new TdesEngine());

            (var csv, var writer) = GetCSVWriter("permutation");

            var keys = new List<BitString>
            {
                new BitString("1046913489980131"),
                new BitString("1007103489988020"),
                new BitString("10071034c8980120"),
                new BitString("1046103489988020"),
                new BitString("1086911519190101"),
                new BitString("1086911519580101"),
                new BitString("5107b01519580101"),
                new BitString("1007b01519190101"),
                new BitString("3107915498080101"),
                new BitString("3107919498080101"),
                new BitString("10079115b9080140"),
                new BitString("3107911598080140"),
                new BitString("1007d01589980101"),
                new BitString("9107911589980101"),
                new BitString("9107d01589190101"),
                new BitString("1007d01598980120"),
                new BitString("1007940498190101"),
                new BitString("0107910491190401"),
                new BitString("0107910491190101"),
                new BitString("0107940491190401"),
                new BitString("19079210981a0101"),
                new BitString("1007911998190801"),
                new BitString("10079119981a0801"),
                new BitString("1007921098190101"),
                new BitString("100791159819010b"),
                new BitString("1004801598190101"),
                new BitString("1004801598190102"),
                new BitString("1004801598190108"),
                new BitString("1002911598100104"),
                new BitString("1002911598190104"),
                new BitString("1002911598100201"),
                new BitString("1002911698100101")
            };

            for (var i = 0; i < keys.Count; i += 2)
            {
                kats.Add(new AlgoArrayResponse
                {
                    PlainText = BitString.Zeroes(64),
                    Key1 = keys[i],
                    Key2 = new BitString("0101010101010101"),
                    Key3 = keys[i+1],
                    IV = BitString.Zeroes(64)
                });
            }

            kats.ForEach(fe =>
            {
                var param = new ModeBlockCipherParameters(
                    BlockCipherDirections.Decrypt,
                    fe.IV,
                    fe.Keys,
                    fe.PlainText
                );

                var result = ecb.ProcessPayload(param);

                fe.CipherText = result.Result;
            });

            csv.WriteRecords(kats);
            writer.Close();
        }

        private static void GenerateSubstitution()
        {

        }

        private static void GenerateInversePermutation()
        {
            var original = new AlgoArrayResponse
            {
                PlainText = BitString.Zeroes(64),
                CipherText = BitString.Zeroes(64),
                IV = BitString.Zeroes(64),
                Key1 = new BitString("0000000000000000").ToOddParityBitString(),
                Key2 = new BitString("0707070707070707").ToOddParityBitString(),
                Key3 = new BitString("FEFEFEFEFEFEFEFE").ToOddParityBitString()
            };

            var kats = new List<AlgoArrayResponse>();
            var ecb = new EcbBlockCipher(new TdesEngine());

            (var csv, var writer) = GetCSVWriter("inverse-permutation");

            var flipped = FlipEachBit(original.CipherText);
            flipped.ForEach(fe =>
            {
                var param = new ModeBlockCipherParameters(
                    BlockCipherDirections.Decrypt,
                    original.IV,
                    original.Keys,
                    fe
                );

                var result = ecb.ProcessPayload(param);

                kats.Add(new AlgoArrayResponse
                {
                    Keys = original.Keys,
                    PlainText = result.Result,
                    IV = original.IV,
                    CipherText = fe
                });
            });

            csv.WriteRecords(kats);
            writer.Close();
        }

        private static void GenerateVariableText()
        {
            var original = new AlgoArrayResponse
            {
                PlainText = BitString.Zeroes(64),
                CipherText = BitString.Zeroes(64),
                IV = BitString.Zeroes(64),
                Key1 = new BitString("0000000000000000").ToOddParityBitString(),
                Key2 = new BitString("0707070707070707").ToOddParityBitString(),
                Key3 = new BitString("FEFEFEFEFEFEFEFE").ToOddParityBitString()
            };

            var kats = new List<AlgoArrayResponse>();
            var ecb = new EcbBlockCipher(new TdesEngine());

            (var csv, var writer) = GetCSVWriter("variable-text");

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

        private static void GenerateVariableKey()
        {
            var original = new AlgoArrayResponse
            {
                PlainText = BitString.Zeroes(64),
                CipherText = BitString.Zeroes(64),
                IV = BitString.Zeroes(64),
                Key1 = new BitString("0000000000000000").ToOddParityBitString(),
                Key2 = new BitString("0707070707070707").ToOddParityBitString(),
                Key3 = new BitString("FEFEFEFEFEFEFEFE").ToOddParityBitString()
            };

            var kats = new List<AlgoArrayResponse>();
            var ecb = new EcbBlockCipher(new TdesEngine());

            (var csv, var writer) = GetCSVWriter("variable-key");

            var flipped = FlipEachBit(original.Keys);
            flipped.ForEach(fe =>
            {
                var key1 = fe.GetMostSignificantBits(64);
                var key2 = fe.MSBSubstring(64, 64);
                var key3 = fe.GetLeastSignificantBits(64);

                fe = key1.ToOddParityBitString().ConcatenateBits(key2.ToOddParityBitString()).ConcatenateBits(key3.ToOddParityBitString());

                var param = new ModeBlockCipherParameters(
                    BlockCipherDirections.Encrypt,
                    original.IV,
                    fe,
                    original.PlainText
                );

                var result = ecb.ProcessPayload(param);

                var algoArray = new AlgoArrayResponse
                {
                    Keys = fe,
                    PlainText = original.PlainText,
                    IV = original.IV,
                    CipherText = result.Result
                };

                if (!kats.Exists(kat => kat.Keys.Equals(algoArray.Keys)))
                {
                    kats.Add(algoArray);
                }
            });

            csv.WriteRecords(kats);
            writer.Close();
        }

        private static (CsvWriter, StreamWriter) GetCSVWriter(string fileName)
        {
            var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            path = Path.Combine(path, $@"..\..\..\{fileName}.csv");
            var writer = new StreamWriter(path);
            var csv = new CsvWriter(writer);
            csv.Configuration.TypeConverterCache.AddConverter<BitString>(new BitStringConverter());

            return (csv, writer);
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
