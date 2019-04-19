using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using CsvHelper;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES.KATs;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Math;

namespace AesKatGenerator
{
    class Program
    {
        private static CbcCtsBlockCipher _cipher = new CbcCtsBlockCipher(new AesEngine());

        static void Main(string[] args)
        {
            Console.WriteLine("Generating KAT data.");

            GFSBox128BitKey();
            GFSBox192BitKey();
            GFSBox256BitKey();

            SBox128BitKey();
            SBox192BitKey();
            SBox256BitKey();

            VarTxt128BitKey();
            VarTxt192BitKey();
            VarTxt256BitKey();

            VarKey128BitKey();
            VarKey192BitKey();
            VarKey256BitKey();
        }

        private static void GFSBox128BitKey()
        {
            var baseData = KATData.GetGFSBox128BitKey();

            RecalculateKat(baseData, MethodBase.GetCurrentMethod().Name);
        }

        private static void GFSBox192BitKey()
        {
            var baseData = KATData.GetGFSBox192BitKey();

            RecalculateKat(baseData, MethodBase.GetCurrentMethod().Name);
        }

        private static void GFSBox256BitKey()
        {
            var baseData = KATData.GetGFSBox256BitKey();

            RecalculateKat(baseData, MethodBase.GetCurrentMethod().Name);
        }

        private static void SBox128BitKey()
        {
            var baseData = KATData.GetKeySBox128BitKey();

            RecalculateKat(baseData, MethodBase.GetCurrentMethod().Name);
        }

        private static void SBox192BitKey()
        {
            var baseData = KATData.GetKeySBox192BitKey();

            RecalculateKat(baseData, MethodBase.GetCurrentMethod().Name);
        }

        private static void SBox256BitKey()
        {
            var baseData = KATData.GetKeySBox256BitKey();

            RecalculateKat(baseData, MethodBase.GetCurrentMethod().Name);
        }

        private static void VarTxt128BitKey()
        {
            var baseData = KATData.GetVarTxt128BitKey();

            RecalculateKat(baseData, MethodBase.GetCurrentMethod().Name);
        }

        private static void VarTxt192BitKey()
        {
            var baseData = KATData.GetVarTxt192BitKey();

            RecalculateKat(baseData, MethodBase.GetCurrentMethod().Name);
        }

        private static void VarTxt256BitKey()
        {
            var baseData = KATData.GetVarTxt256BitKey();

            RecalculateKat(baseData, MethodBase.GetCurrentMethod().Name);
        }

        private static void VarKey128BitKey()
        {
            var baseData = KATData.GetVarKey128BitKey();

            RecalculateKat(baseData, MethodBase.GetCurrentMethod().Name);
        }

        private static void VarKey192BitKey()
        {
            var baseData = KATData.GetVarKey192BitKey();

            RecalculateKat(baseData, MethodBase.GetCurrentMethod().Name);
        }

        private static void VarKey256BitKey()
        {
            var baseData = KATData.GetVarKey256BitKey();

            RecalculateKat(baseData, MethodBase.GetCurrentMethod().Name);
        }

        private static void RecalculateKat(List<AlgoArrayResponse> baseData, string filename)
        {
            // Transform the data by making the "full block" pt a partial block
            baseData.ForEach(fe => fe.PlainText = fe.PlainText.ConcatenateBits(new BitString(64)));

            // Perform the crypto operation and update the ct
            baseData.ForEach(fe =>
            {
                var param = new ModeBlockCipherParameters(
                    BlockCipherDirections.Encrypt,
                    fe.IV.GetDeepCopy(),
                    fe.Key,
                    fe.PlainText
                );

                var result = _cipher.ProcessPayload(param);

                fe.CipherText = result.Result;
            });

            // Write the CSV containing the new KAT data
            var (csv, writer) = GetCSVWriter(filename);
            csv.WriteRecords(baseData);
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
    }
}
