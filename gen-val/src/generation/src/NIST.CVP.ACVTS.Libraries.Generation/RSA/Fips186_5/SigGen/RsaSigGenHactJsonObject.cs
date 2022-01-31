using System;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.Fips186_5.SigGen
{
    public class RsaSigGenHactJsonObject
    {
        public string Algorithm { get; set; }
        public int PrimeLen { get; set; }
        public int PublicKeyLen { get; set; }
        public int PrivateKeyLen { get; set; }
        public RsaSigGenHactTestCaseObject[] EncryptParameters { get; set; }
    }

    public class RsaSigGenHactTestCaseObject
    {
        public int MsgLen { get; set; }
    }

    public static class HactJsonFactory
    {
        private static readonly Dictionary<int, RsaSigGenHactJsonObject> Map = new()
        {
            {
                2048,
                new RsaSigGenHactJsonObject
                {
                    Algorithm = "rsa-enc",
                    PrimeLen = 2048,
                    PrivateKeyLen = 2048,
                    PublicKeyLen = 2048,
                    EncryptParameters = new[]
                    {
                        new RsaSigGenHactTestCaseObject
                        {
                            MsgLen = 1960
                        },
                        new RsaSigGenHactTestCaseObject
                        {
                            MsgLen = 8
                        },
                    }
                }
            }
        };

        public static RsaSigGenHactJsonObject GetHactParameters(int modulo)
        {
            if (!Map.TryFirst(m => m.Key == modulo, out var result))
            {
                throw new ArgumentException($"Could not find key matching {modulo}", nameof(modulo));
            }

            return result.Value;
        }
    }
}
