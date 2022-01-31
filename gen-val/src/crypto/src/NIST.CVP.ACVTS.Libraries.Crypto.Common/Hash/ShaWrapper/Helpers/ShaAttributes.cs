using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers
{
    /// <summary>
    /// Get SHA information
    /// </summary>
    public static class ShaAttributes
    {
        private static List<(ModeValues mode, DigestSizes digestSize, int outputLen, int blockSize, BigInteger maxMessageSize, int processingLen, string name)> shaAttributes =
            new List<(ModeValues mode, DigestSizes digestSize, int outputLen, int blockSize, BigInteger maxMessageSize, int processingLen, string name)>()
            {
                (ModeValues.SHA1, DigestSizes.d160, 160, 512, ((BigInteger)1 << 64) - 1, 160, "SHA-1"),
                (ModeValues.SHA2, DigestSizes.d224, 224, 512, ((BigInteger)1 << 64) - 1, 224, "SHA2-224"),
                (ModeValues.SHA2, DigestSizes.d256, 256, 512, ((BigInteger)1 << 64) - 1, 256, "SHA2-256"),
                (ModeValues.SHA2, DigestSizes.d384, 384, 1024, ((BigInteger)1 << 128) - 1, 384, "SHA2-384"),
                (ModeValues.SHA2, DigestSizes.d512, 512, 1024, ((BigInteger)1 << 128) - 1, 512, "SHA2-512"),
                (ModeValues.SHA2, DigestSizes.d512t224, 224, 1024, ((BigInteger)1 << 128) - 1, 512, "SHA2-512/224"),
                (ModeValues.SHA2, DigestSizes.d512t256, 256, 1024, ((BigInteger)1 << 128) - 1, 512, "SHA2-512/256"),
                (ModeValues.SHA3, DigestSizes.d224, 224, 1152, -1, 224, "SHA3-224"), // no limit
                (ModeValues.SHA3, DigestSizes.d256, 256, 1088, -1, 256, "SHA3-256"), // no limit
                (ModeValues.SHA3, DigestSizes.d384, 384, 832, -1, 384, "SHA3-384"), // no limit
                (ModeValues.SHA3, DigestSizes.d512, 512, 576, -1, 512, "SHA3-512"), // no limit
                (ModeValues.SHAKE, DigestSizes.d128, 128, 1344, -1, 128, "SHAKE-128"), // no limit, outputSize is "common output size", normally defined by the function calling SHAKE
                (ModeValues.SHAKE, DigestSizes.d256, 256, 1088, -1, 256, "SHAKE-256") // no limit, outputSize is "common output size", normally defined by the function calling SHAKE
            };

        private static List<(HashFunctions hashFunction, ModeValues modeValue, DigestSizes digestSizes)>
            _hashFunctionsMap = new List<(HashFunctions hashFunction, ModeValues modeValue, DigestSizes digestSizes)>()
            {
                (HashFunctions.Sha1, ModeValues.SHA1, DigestSizes.d160),
                (HashFunctions.Sha2_d224, ModeValues.SHA2, DigestSizes.d224),
                (HashFunctions.Sha2_d256, ModeValues.SHA2, DigestSizes.d256),
                (HashFunctions.Sha2_d384, ModeValues.SHA2, DigestSizes.d384),
                (HashFunctions.Sha2_d512, ModeValues.SHA2, DigestSizes.d512),
                (HashFunctions.Sha2_d512t224, ModeValues.SHA2, DigestSizes.d512t224),
                (HashFunctions.Sha2_d512t256, ModeValues.SHA2, DigestSizes.d512t256),
                (HashFunctions.Sha3_d224, ModeValues.SHA3, DigestSizes.d224),
                (HashFunctions.Sha3_d256, ModeValues.SHA3, DigestSizes.d256),
                (HashFunctions.Sha3_d384, ModeValues.SHA3, DigestSizes.d384),
                (HashFunctions.Sha3_d512, ModeValues.SHA3, DigestSizes.d512),
            };

        public static List<(ModeValues mode, DigestSizes digestSize, int outputLen, int blockSize, BigInteger maxMessageSize, int processingLen, string name)> GetShaAttributes()
        {
            return shaAttributes;
        }

        public static List<string> GetShaNames()
        {
            return shaAttributes
                .Select<(ModeValues, DigestSizes, int, int, BigInteger, int, string name), string>(s => s.name).ToList();
        }

        public static (ModeValues mode, DigestSizes digestSize, int outputLen, int blockSize, BigInteger maxMessageSize, int processingLen, string name) GetShaAttributes(ModeValues mode, DigestSizes digestSize)
        {
            if (!GetShaAttributes()
                .TryFirst(w => w.mode == mode && w.digestSize == digestSize, out var result))
            {
                throw new ArgumentException($"Invalid {nameof(mode)}/{nameof(digestSize)} combination");
            }

            return result;
        }

        public static (ModeValues mode, DigestSizes digestSize, int outputLen, int blockSize, BigInteger maxMessageSize, int processingLen, string name) GetShaAttributes(HashFunctions hashFunction)
        {
            var hf = GetHashFunctionFromEnum(hashFunction);

            return GetShaAttributes(hf.Mode, hf.DigestSize);
        }

        public static (ModeValues mode, DigestSizes digestSize, int outputLen, int blockSize, BigInteger maxMessageSize, int processingLen, string name) GetShaAttributes(string name)
        {
            if (!GetShaAttributes()
                .TryFirst(w => w.name.Equals(name, StringComparison.OrdinalIgnoreCase), out var result))
            {
                throw new ArgumentException($"Invalid sha {nameof(name)}");
            }

            return result;
        }

        public static HashFunction GetHashFunctionFromName(string name)
        {
            var attributes = GetShaAttributes(name);
            return new HashFunction(attributes.mode, attributes.digestSize);
        }

        public static HashFunction GetHashFunctionFromEnum(HashFunctions hashFunction)
        {
            if (!_hashFunctionsMap
                .TryFirst(w => w.hashFunction == hashFunction, out var result))
            {
                throw new ArgumentException($"Invalid {nameof(hashFunction)}.");
            }

            return new HashFunction(result.modeValue, result.digestSizes);
        }

        public static BitString HashFunctionToBits(DigestSizes digestSize)
        {
            switch (digestSize)
            {
                case DigestSizes.d128:
                    return new BitString("32");
                case DigestSizes.d160:
                    return new BitString("33");
                case DigestSizes.d224:
                    return new BitString("38");
                case DigestSizes.d256:
                    return new BitString("34");
                case DigestSizes.d384:
                    return new BitString("36");
                case DigestSizes.d512:
                    return new BitString("35");
                case DigestSizes.d512t224:
                    return new BitString("39");
                case DigestSizes.d512t256:
                    return new BitString("3a");     // Value taken from CAVS was previously 0x40, should be 0x3a
                default:
                    throw new Exception("Bad digest size");
            }
        }

        public static DigestSizes StringToDigest(string digestSize)
        {
            switch (digestSize.ToLower())
            {
                case "128":
                    return DigestSizes.d128;
                case "160":
                    return DigestSizes.d160;
                case "224":
                    return DigestSizes.d224;
                case "256":
                    return DigestSizes.d256;
                case "384":
                    return DigestSizes.d384;
                case "512":
                    return DigestSizes.d512;
                case "512/224":
                    return DigestSizes.d512t224;
                case "512/256":
                    return DigestSizes.d512t256;
                default:
                    return DigestSizes.NONE;
            }
        }

        public static ModeValues StringToMode(string mode)
        {
            switch (mode.ToLower())
            {
                case "sha1":
                case "sha-1":
                    return ModeValues.SHA1;
                case "sha2":
                case "sha2-224":
                case "sha2-256":
                case "sha2-384":
                case "sha2-512":
                case "sha2-512/224":
                case "sha2-512/256":
                    return ModeValues.SHA2;

                case "sha3":
                case "sha-3":
                    return ModeValues.SHA3;

                case "shake":
                    return ModeValues.SHAKE;

                default:
                    throw new Exception("Bad mode for SHA");
            }
        }
    }
}
