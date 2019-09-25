using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Pools.Enums;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NIST.CVP.Pools.Models;

namespace ConfigFileGenerator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var DEFAULT_MAX_CAPACITY = 50;

            var fullParamList = new List<PoolProperties>();
            var rsaParamList = new List<PoolProperties>();
            
            #region SHA1/2
            var hashAlgs = new []
            {
                new HashFunction(ModeValues.SHA1, DigestSizes.d160),
                new HashFunction(ModeValues.SHA2, DigestSizes.d224),
                new HashFunction(ModeValues.SHA2, DigestSizes.d256),
                new HashFunction(ModeValues.SHA2, DigestSizes.d384),
                new HashFunction(ModeValues.SHA2, DigestSizes.d512),
                new HashFunction(ModeValues.SHA2, DigestSizes.d512t224),
                new HashFunction(ModeValues.SHA2, DigestSizes.d512t256), 
            };

            foreach (var hashAlg in hashAlgs)
            {
                fullParamList.Add(new PoolProperties
                {
                    MaxCapacity = DEFAULT_MAX_CAPACITY,
                    PoolName = Stringify(SHAEnumHelpers.HashFunctionToString(hashAlg).Replace("/", "-")),
                    PoolType = new ParameterHolder
                    {
                        Type = PoolTypes.SHA_MCT,
                        Parameters = new ShaParameters
                        {
                            HashFunction = hashAlg,
                            MessageLength = SHAEnumHelpers.DigestSizeToInt(hashAlg.DigestSize)
                        }
                    }
                });
            }
            #endregion SHA1/2

            #region SHA3
            var sha3DigestSizes = new[] {224, 256, 384, 512};

            foreach (var digestSize in sha3DigestSizes)
            {
                fullParamList.Add(new PoolProperties
                {
                    MaxCapacity = DEFAULT_MAX_CAPACITY,
                    PoolName = Stringify("sha3", $"{digestSize}"),
                    PoolType = new ParameterHolder
                    {
                        Type = PoolTypes.SHA3_MCT,
                        Parameters = new Sha3Parameters
                        {
                            HashFunction = new NIST.CVP.Crypto.Common.Hash.SHA3.HashFunction(digestSize, digestSize*2, false),
                            MessageLength = digestSize
                        }
                    }
                });
            }

            var shakeDigestSizes = new[] {128, 256};

            foreach (var digestSize in shakeDigestSizes)
            {
                fullParamList.Add(new PoolProperties
                {
                    MaxCapacity = DEFAULT_MAX_CAPACITY,
                    PoolName = Stringify("shake", $"{digestSize}"),
                    PoolType = new ParameterHolder
                    {
                        Type = PoolTypes.SHA3_MCT,
                        Parameters = new Sha3Parameters
                        {
                            HashFunction = new NIST.CVP.Crypto.Common.Hash.SHA3.HashFunction(digestSize, digestSize * 2, true),
                            MessageLength = digestSize
                        }
                    }
                });
            }
            #endregion SHA3

            #region SHA3-Derived
            #endregion SHA3-Derived

            #region RSA
            var keyModes = EnumHelpers.GetEnumDescriptions<PrimeGenModes>();
            //var modulo = new[] {1024, 2048, 3072, 4096};
            var modulo = new[] {4096};
            var primeTestModes = EnumHelpers.GetEnumDescriptions<PrimeTestModes>();
            var hashAlgsWrap = new List<NIST.CVP.Crypto.Common.Hash.ShaWrapper.HashFunction>
            {
                new NIST.CVP.Crypto.Common.Hash.ShaWrapper.HashFunction(NIST.CVP.Crypto.Common.Hash.ShaWrapper.ModeValues.SHA1, NIST.CVP.Crypto.Common.Hash.ShaWrapper.DigestSizes.d160),
                new NIST.CVP.Crypto.Common.Hash.ShaWrapper.HashFunction(NIST.CVP.Crypto.Common.Hash.ShaWrapper.ModeValues.SHA2, NIST.CVP.Crypto.Common.Hash.ShaWrapper.DigestSizes.d224),
                new NIST.CVP.Crypto.Common.Hash.ShaWrapper.HashFunction(NIST.CVP.Crypto.Common.Hash.ShaWrapper.ModeValues.SHA2, NIST.CVP.Crypto.Common.Hash.ShaWrapper.DigestSizes.d256),
                new NIST.CVP.Crypto.Common.Hash.ShaWrapper.HashFunction(NIST.CVP.Crypto.Common.Hash.ShaWrapper.ModeValues.SHA2, NIST.CVP.Crypto.Common.Hash.ShaWrapper.DigestSizes.d384),
                new NIST.CVP.Crypto.Common.Hash.ShaWrapper.HashFunction(NIST.CVP.Crypto.Common.Hash.ShaWrapper.ModeValues.SHA2, NIST.CVP.Crypto.Common.Hash.ShaWrapper.DigestSizes.d512),
                new NIST.CVP.Crypto.Common.Hash.ShaWrapper.HashFunction(NIST.CVP.Crypto.Common.Hash.ShaWrapper.ModeValues.SHA2, NIST.CVP.Crypto.Common.Hash.ShaWrapper.DigestSizes.d512t224),
                new NIST.CVP.Crypto.Common.Hash.ShaWrapper.HashFunction(NIST.CVP.Crypto.Common.Hash.ShaWrapper.ModeValues.SHA2, NIST.CVP.Crypto.Common.Hash.ShaWrapper.DigestSizes.d512t256)
            };

            foreach (var keyMode in keyModes)
            {
                foreach (var mod in modulo)
                {
                    foreach (var primeTestMode in primeTestModes)
                    {
                        foreach (var hashAlg in hashAlgsWrap)
                        {
                            rsaParamList.Add(new PoolProperties
                            {
                                MaxCapacity = DEFAULT_MAX_CAPACITY,
                                PoolName = Stringify("rsa", keyMode.Replace(".", ""), $"{mod}", primeTestMode.Replace(".", ""), hashAlg.Name.Replace("/", "")),
                                PoolType = new ParameterHolder
                                {
                                    Type = PoolTypes.RSA_KEY,
                                    Parameters = new RsaKeyParameters
                                    {
                                        HashAlg = hashAlg,
                                        KeyMode = EnumHelpers.GetEnumFromEnumDescription<PrimeGenModes>(keyMode),
                                        Modulus = mod,
                                        PrimeTest = EnumHelpers.GetEnumFromEnumDescription<PrimeTestModes>(primeTestMode),
                                        PublicExponentMode = PublicExponentModes.Random
                                    }
                                }
                            });
                        }
                    }
                }
            }
            #endregion RSA

            #region DSA
            var lnPairs = new List<(int L, int N)>
            {
                (1024, 160),
                (2048, 224),
                (2048, 256),
                (3072, 256)
            };

            var pqModes = EnumHelpers.GetEnumDescriptions<PrimeGenMode>().Except(new[] { "none" }).ToArray();
            var gModes = EnumHelpers.GetEnumDescriptions<GeneratorGenMode>().Except(new[] { "none" }).ToArray();

            foreach (var lnPair in lnPairs)
            {
                foreach (var pqMode in pqModes)
                {
                    foreach (var gMode in gModes)
                    {
                        foreach (var hashAlg in hashAlgsWrap)
                        {
                            fullParamList.Add(new PoolProperties
                            {
                                MaxCapacity = DEFAULT_MAX_CAPACITY,
                                PoolName = Stringify("dsa", $"{lnPair.L}", $"{lnPair.N}", pqMode, gMode, hashAlg.Name.Replace("/", "-")),
                                PoolType = new ParameterHolder
                                {
                                    Type = PoolTypes.DSA_PQG,
                                    Parameters = new DsaDomainParametersParameters
                                    {
                                        Disposition = "none",
                                        GGenMode = EnumHelpers.GetEnumFromEnumDescription<GeneratorGenMode>(gMode),
                                        PQGenMode = EnumHelpers.GetEnumFromEnumDescription<PrimeGenMode>(pqMode),
                                        L = lnPair.L,
                                        N = lnPair.N,
                                        HashAlg = hashAlg
                                    }
                                }
                            });
                        }
                    }
                }
            }
            #endregion DSA

            #region ECDSA
            var curves = EnumHelpers.GetEnumDescriptions<Curve>();

            foreach (var curve in curves)
            {
                fullParamList.Add(new PoolProperties
                {
                    MaxCapacity = DEFAULT_MAX_CAPACITY,
                    PoolName = Stringify("ecdsa", curve),
                    PoolType = new ParameterHolder
                    {
                        Type = PoolTypes.ECDSA_KEY,
                        Parameters = new EcdsaKeyParameters
                        {
                            Curve = EnumHelpers.GetEnumFromEnumDescription<Curve>(curve),
                            Disposition = EcdsaKeyDisposition.None
                        }
                    }
                });
            }
            #endregion ECDSA
            
            #region AES
            var aesModes = new[] {"ecb", "cbc", "ofb", "cfbbit", "cfbbyte", "cfbblock"};
            var keySizes = new[] {128, 192, 256};
            var directions = new[] {"encrypt", "decrypt"};

            foreach (var aesMode in aesModes)
            {
                foreach (var keySize in keySizes)
                {
                    foreach (var direction in directions)
                    {
                        fullParamList.Add(new PoolProperties
                        {
                            MaxCapacity = DEFAULT_MAX_CAPACITY,
                            PoolName = Stringify("aes", aesMode, $"{keySize}", direction),
                            PoolType = new ParameterHolder
                            {
                                Type = PoolTypes.AES_MCT,
                                Parameters = new AesParameters
                                {
                                    Direction = direction,
                                    KeyLength = keySize,
                                    DataLength = aesMode.Contains("bit") ? 1 : aesMode.Contains("byte") ? 8 : 128,
                                    Mode = EnumHelpers.GetEnumFromEnumDescription<BlockCipherModesOfOperation>(aesMode)
                                }
                            }
                        });
                    }
                }
            }
            #endregion AES

            #region TDES
            var tdesModes = new[] {"ecb", "cbc", "cbci", "ofbi", "cfbbit", "cfbbyte", "cfbblock", "cfbpbit", "cfbpbyte", "cfbpblock"};
            var keyingOptions = new[] {1, 2};

            foreach (var tdesMode in tdesModes)
            {
                foreach (var keyingOption in keyingOptions)
                {
                    foreach (var direction in directions)
                    {
                        fullParamList.Add(new PoolProperties
                        {
                            MaxCapacity = DEFAULT_MAX_CAPACITY,
                            PoolName = Stringify("tdes", tdesMode, direction, "keyingoption", $"{keyingOption}"),
                            PoolType = new ParameterHolder
                            {
                                Type = PoolTypes.TDES_MCT,
                                Parameters = new TdesParameters
                                {
                                    KeyingOption = keyingOption,
                                    Direction = direction,
                                    DataLength = tdesMode.Contains("bit") ? 1 : tdesMode.Contains("byte") ? 8 : 64,
                                    Mode = EnumHelpers.GetEnumFromEnumDescription<BlockCipherModesOfOperation>(tdesMode)
                                }
                            }
                        });
                    }
                }
            }
            #endregion TDES

            var result = JsonConvert.SerializeObject(rsaParamList, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                Converters = new List<JsonConverter>
                {
                    new StringEnumConverter()
                }
            });

            var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            path = Path.Combine(path, $@"../../../poolConfig2.json");
            Console.WriteLine(path);
            File.WriteAllText(path, result);
        }

        private static string Stringify(params string[] list)
        {
            var result = "";
            foreach (var param in list)
            {
                result += param;
                result += "-";
            }

            result = result.Remove(result.Length-1);

            return result.ToLower();
        }
    }
}
