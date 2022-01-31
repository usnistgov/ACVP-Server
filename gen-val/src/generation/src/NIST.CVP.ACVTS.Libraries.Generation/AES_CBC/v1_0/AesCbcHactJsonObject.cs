using System;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_CBC.v1_0
{
    public class AesCbcHactJsonObject
    {
        public string Algorithm { get; set; }
        public int KeyLen { get; set; }
        public AesCbcHactTestCaseObject[] EncryptParameters { get; set; }
        public AesCbcHactTestCaseObject[] DecryptParameters { get; set; }
    }

    public class AesCbcHactTestCaseObject
    {
        public int NumBlocks { get; set; }
    }

    public static class HactJsonFactory
    {
        private static readonly Dictionary<int, AesCbcHactJsonObject> Map = new()
        {
            {
                128,
                new AesCbcHactJsonObject
                {
                    Algorithm = "aes-cbc",
                    KeyLen = 128,
                    EncryptParameters = new[]
                    {
                        new AesCbcHactTestCaseObject
                        {
                            NumBlocks = 0
                        },
                        new AesCbcHactTestCaseObject
                        {
                            NumBlocks = 1
                        },
                        new AesCbcHactTestCaseObject
                        {
                            NumBlocks = 8
                        },
                    },
                    DecryptParameters = new[]
                    {
                        new AesCbcHactTestCaseObject
                        {
                            NumBlocks = 0
                        },
                        new AesCbcHactTestCaseObject
                        {
                            NumBlocks = 1
                        },
                        new AesCbcHactTestCaseObject
                        {
                            NumBlocks = 8
                        },
                    }
                }
            },
            {
                192,
                new AesCbcHactJsonObject
                {
                    Algorithm = "aes-cbc",
                    KeyLen = 192,
                    EncryptParameters = new[]
                    {
                        new AesCbcHactTestCaseObject
                        {
                            NumBlocks = 0
                        },
                        new AesCbcHactTestCaseObject
                        {
                            NumBlocks = 1
                        },
                        new AesCbcHactTestCaseObject
                        {
                            NumBlocks = 8
                        },
                    },
                    DecryptParameters = new[]
                    {
                        new AesCbcHactTestCaseObject
                        {
                            NumBlocks = 0
                        },
                        new AesCbcHactTestCaseObject
                        {
                            NumBlocks = 1
                        },
                        new AesCbcHactTestCaseObject
                        {
                            NumBlocks = 8
                        },
                    }
                }
            },
            {
                256,
                new AesCbcHactJsonObject
                {
                    Algorithm = "aes-cbc",
                    KeyLen = 256,
                    EncryptParameters = new[]
                    {
                        new AesCbcHactTestCaseObject
                        {
                            NumBlocks = 0
                        },
                        new AesCbcHactTestCaseObject
                        {
                            NumBlocks = 1
                        },
                        new AesCbcHactTestCaseObject
                        {
                            NumBlocks = 8
                        },
                    },
                    DecryptParameters = new[]
                    {
                        new AesCbcHactTestCaseObject
                        {
                            NumBlocks = 0
                        },
                        new AesCbcHactTestCaseObject
                        {
                            NumBlocks = 1
                        },
                        new AesCbcHactTestCaseObject
                        {
                            NumBlocks = 8
                        },
                    }
                }
            },
        };

        public static AesCbcHactJsonObject GetHactParameters(int keyLength)
        {
            if (!Map.TryFirst(m => m.Key == keyLength, out var result))
            {
                throw new ArgumentException($"Could not find key matching {keyLength}", nameof(keyLength));
            }

            return result.Value;
        }
    }
}
