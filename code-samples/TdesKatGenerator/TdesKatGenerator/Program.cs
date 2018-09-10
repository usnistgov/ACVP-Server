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
            var kats = new List<AlgoArrayResponse>();
            var ecb = new EcbBlockCipher(new TdesEngine());

            (var csv, var writer) = GetCSVWriter("substitution");

            var keys = new List<BitString>
            {
                new BitString("7ca110454a1a6e57"),
                new BitString("0131d9619dc1376e"),
                new BitString("07a1133e4a0b2686"),
                new BitString("3849674c2602319e"),
                new BitString("04b915ba43feb5b6"),
                new BitString("0113b970fd34f2ce"),
                new BitString("0170f175468fb5e6"),
                new BitString("43297fad38e373fe"),
                new BitString("07a7137045da2a16"),
                new BitString("04689104c2fd3b2f"),
                new BitString("37d06bb516cb7546"),
                new BitString("1f08260d1ac2465e"),
                new BitString("584023641aba6176"),
                new BitString("025816164629b007"),
                new BitString("49793ebc79b3258f"),
                new BitString("4fb05e1515ab73a7"),
                new BitString("49e95d6d4ca229bf"),
                new BitString("018310dc409b26d6"),
                new BitString("1c587f1c13924fef")
            };

            var plainTexts = new List<BitString>
            {
                new BitString("01a1d6d039776742"),
                new BitString("5cd54ca83def57da"),
                new BitString("0248d43806f67172"),
                new BitString("51454b582ddf440a"),
                new BitString("42fd443059577fa2"),
                new BitString("059b5e0851cf143a"),
                new BitString("0756d8e0774761d2"),
                new BitString("762514b829bf486a"),
                new BitString("3bdd119049372802"),
                new BitString("26955f6835af609a"),
                new BitString("164d5e404f275232"),
                new BitString("6b056e18759f5cca"),
                new BitString("004bd6ef09176062"),
                new BitString("480d39006ee762f2"),
                new BitString("437540c8698f3cfa"),
                new BitString("072d43a077075292"),
                new BitString("02fe55778117f12a"),
                new BitString("1d9d5c5018f728c2"),
                new BitString("305532286d6f295a")
            };

            for (var i = 0; i < keys.Count; i ++)
            {
                kats.Add(new AlgoArrayResponse
                {
                    PlainText = plainTexts[i],
                    Key1 = keys[i],
                    Key2 = new BitString("0101010101010101").ToOddParityBitString(),
                    Key3 = new BitString("0202020202020202").ToOddParityBitString(),
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
