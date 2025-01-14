using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;

/// <summary>
/// Get SHA information
/// </summary>
public static class ShaAttributes
{
    private static List<(ModeValues mode, DigestSizes digestSize, int outputLen, int blockSize, BigInteger maxMessageSize, int processingLen, byte[] OID, string name)> shaAttributes =
        new ()
        {
            (ModeValues.SHA1, DigestSizes.d160, 160, 512, ((BigInteger)1 << 64) - 1, 160, new byte[] {0x00}, "SHA-1"),
                
            (ModeValues.SHA2, DigestSizes.d224, 224, 512, ((BigInteger)1 << 64) - 1, 224, new byte[] {0x06, 0x09, 0x60, 0x86, 0x48, 0x01, 0x65, 0x03, 0x04, 0x02, 0x04}, "SHA2-224"),
            (ModeValues.SHA2, DigestSizes.d256, 256, 512, ((BigInteger)1 << 64) - 1, 256, new byte[] {0x06, 0x09, 0x60, 0x86, 0x48, 0x01, 0x65, 0x03, 0x04, 0x02, 0x01}, "SHA2-256"),
            (ModeValues.SHA2, DigestSizes.d384, 384, 1024, ((BigInteger)1 << 128) - 1, 384, new byte[] {0x06, 0x09, 0x60, 0x86, 0x48, 0x01, 0x65, 0x03, 0x04, 0x02, 0x02}, "SHA2-384"),
            (ModeValues.SHA2, DigestSizes.d512, 512, 1024, ((BigInteger)1 << 128) - 1, 512, new byte[] {0x06, 0x09, 0x60, 0x86, 0x48, 0x01, 0x65, 0x03, 0x04, 0x02, 0x03}, "SHA2-512"),
            (ModeValues.SHA2, DigestSizes.d512t224, 224, 1024, ((BigInteger)1 << 128) - 1, 512, new byte[] {0x06, 0x09, 0x60, 0x86, 0x48, 0x01, 0x65, 0x03, 0x04, 0x02, 0x05}, "SHA2-512/224"),
            (ModeValues.SHA2, DigestSizes.d512t256, 256, 1024, ((BigInteger)1 << 128) - 1, 512, new byte[] {0x06, 0x09, 0x60, 0x86, 0x48, 0x01, 0x65, 0x03, 0x04, 0x02, 0x06}, "SHA2-512/256"),
                
            // SHA-3 and SHAKE have no input limit
            (ModeValues.SHA3, DigestSizes.d224, 224, 1152, -1, 224, new byte[] {0x06, 0x09, 0x60, 0x86, 0x48, 0x01, 0x65, 0x03, 0x04, 0x02, 0x07}, "SHA3-224"),
            (ModeValues.SHA3, DigestSizes.d256, 256, 1088, -1, 256, new byte[] {0x06, 0x09, 0x60, 0x86, 0x48, 0x01, 0x65, 0x03, 0x04, 0x02, 0x08}, "SHA3-256"),
            (ModeValues.SHA3, DigestSizes.d384, 384, 832, -1, 384, new byte[] {0x06, 0x09, 0x60, 0x86, 0x48, 0x01, 0x65, 0x03, 0x04, 0x02, 0x09}, "SHA3-384"),
            (ModeValues.SHA3, DigestSizes.d512, 512, 576, -1, 512, new byte[] {0x06, 0x09, 0x60, 0x86, 0x48, 0x01, 0x65, 0x03, 0x04, 0x02, 0x0A}, "SHA3-512"),
                
            // SHAKE has no output limit, but the output size is the common output size
            (ModeValues.SHAKE, DigestSizes.d128, 128, 1344, -1, 128, new byte[] {0x06, 0x09, 0x60, 0x86, 0x48, 0x01, 0x65, 0x03, 0x04, 0x02, 0x0B}, "SHAKE-128"),
            (ModeValues.SHAKE, DigestSizes.d256, 256, 1088, -1, 256, new byte[] {0x06, 0x09, 0x60, 0x86, 0x48, 0x01, 0x65, 0x03, 0x04, 0x02, 0x0C}, "SHAKE-256") 
        };

    private static List<(ModeValues mode, DigestSizes digestSize, int outputLen, int blockSize, BigInteger maxMessageSize, int processingLen, byte[] OID, string name)> xofSignatureAttributes =
        new ()
        {
            (ModeValues.SHAKE, DigestSizes.d128, 256, 1344, -1, 128, new byte[] {0x06, 0x09, 0x60, 0x86, 0x48, 0x01, 0x65, 0x03, 0x04, 0x02, 0x0B}, "SHAKE-128"), // this entry makes SHAKE-128 work as a "Hash" for PSS; FIPS 186-5 requires an outputLen of 256
            (ModeValues.SHAKE, DigestSizes.d256, 512, 1088, -1, 256, new byte[] {0x06, 0x09, 0x60, 0x86, 0x48, 0x01, 0x65, 0x03, 0x04, 0x02, 0x0C}, "SHAKE-256") // this entry makes SHAKE-256 work as a "Hash" for PSS; FIPS 186-5 requires an outputLen of 512
        };
        
    private static List<(HashFunctions hashFunction, ModeValues modeValue, DigestSizes digestSizes)> _hashFunctionsMap = 
        new ()
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
            (HashFunctions.Shake_d128, ModeValues.SHAKE, DigestSizes.d128),
            (HashFunctions.Shake_d256, ModeValues.SHAKE, DigestSizes.d256)
        };
        
    public static List<(ModeValues mode, DigestSizes digestSize, int outputLen, int blockSize, BigInteger maxMessageSize, int processingLen, byte[] OID, string name)> GetShaAttributes()
    {
        return shaAttributes;
    }
        
    public static List<(ModeValues mode, DigestSizes digestSize, int outputLen, int blockSize, BigInteger maxMessageSize, int processingLen, byte[] OID, string name)> GetXofSignatureAttributes()
    {
        return xofSignatureAttributes;
    }
    
    public static List<string> GetShaNames()
    {
        return shaAttributes.Select<(ModeValues, DigestSizes, int, int, BigInteger, int, byte[], string name), string>(s => s.name).ToList();
    }

    public static (ModeValues mode, DigestSizes digestSize, int outputLen, int blockSize, BigInteger maxMessageSize, int processingLen, byte[] OID, string name) GetShaAttributes(ModeValues mode, DigestSizes digestSize)
    {
        if (!GetShaAttributes().TryFirst(w => w.mode == mode && w.digestSize == digestSize, out var result))
        {
            throw new ArgumentException($"Invalid {nameof(mode)}/{nameof(digestSize)} combination");
        }

        return result;
    }

    public static (ModeValues mode, DigestSizes digestSize, int outputLen, int blockSize, BigInteger maxMessageSize, int processingLen, byte[] OID, string name) GetXofPssAttributes(ModeValues mode, DigestSizes digestSize)
    {
        if (!GetXofSignatureAttributes().TryFirst(w => w.mode == mode && w.digestSize == digestSize, out var result))
        {
            throw new ArgumentException($"Invalid {nameof(mode)}/{nameof(digestSize)} combination");
        }

        return result;
    }
        
    public static (ModeValues mode, DigestSizes digestSize, int outputLen, int blockSize, BigInteger maxMessageSize, int processingLen, byte[] OID, string name) GetShaAttributes(HashFunctions hashFunction)
    {
        var hf = GetHashFunctionFromEnum(hashFunction);

        return GetShaAttributes(hf.Mode, hf.DigestSize);
    }

    public static (ModeValues mode, DigestSizes digestSize, int outputLen, int blockSize, BigInteger maxMessageSize, int processingLen, byte[] OID, string name) GetShaAttributes(string name)
    {
        if (!GetShaAttributes().TryFirst(w => w.name.Equals(name, StringComparison.OrdinalIgnoreCase), out var result))
        {
            throw new ArgumentException($"Invalid sha {nameof(name)}");
        }

        return result;
    }

    public static (ModeValues mode, DigestSizes digestSize, int outputLen, int blockSize, BigInteger maxMessageSize, int processingLen, byte[] OID, string name) GetXofPssAttributes(string name)
    {
        if (!GetXofSignatureAttributes().TryFirst(w => w.name.Equals(name, StringComparison.OrdinalIgnoreCase), out var result))
        {
            throw new ArgumentException($"Invalid XOF {nameof(name)}");
        }

        return result;
    }
        
    public static HashFunction GetHashFunctionFromName(string name)
    {
        var attributes = GetShaAttributes(name);
        return new HashFunction(attributes.mode, attributes.digestSize);
    }

    public static HashFunction GetXofPssHashFunctionFromName(string name)
    {
        var attributes = GetXofPssAttributes(name);
        return new HashFunction(attributes.mode, attributes.digestSize, true);
    }
        
    public static int GetXofPssOutputLenFromName(string name)
    {
        var attributes = GetXofPssAttributes(name);
        return attributes.outputLen;
    }
        
    public static HashFunction GetHashFunctionFromEnum(HashFunctions hashFunction)
    {
        if (!_hashFunctionsMap.TryFirst(w => w.hashFunction == hashFunction, out var result))
        {
            throw new ArgumentException($"Invalid {nameof(hashFunction)}.");
        }

        return new HashFunction(result.modeValue, result.digestSizes);
    }

    public static BitString HashFunctionToBits(DigestSizes digestSize)
    {
        return digestSize switch
        {
            DigestSizes.d128 => new BitString("32"),
            DigestSizes.d160 => new BitString("33"),
            DigestSizes.d224 => new BitString("38"),
            DigestSizes.d256 => new BitString("34"),
            DigestSizes.d384 => new BitString("36"),
            DigestSizes.d512 => new BitString("35"),
            DigestSizes.d512t224 => new BitString("39"),
            DigestSizes.d512t256 => new BitString("3a"), // Value taken from CAVS was previously 0x40, should be 0x3a
                
            _ => throw new Exception("Bad digest size")
        };
    }

    public static DigestSizes StringToDigest(string digestSize)
    {
        return digestSize.ToLower() switch
        {
            "128" => DigestSizes.d128,
            "160" => DigestSizes.d160,
            "224" => DigestSizes.d224,
            "256" => DigestSizes.d256,
            "384" => DigestSizes.d384,
            "512" => DigestSizes.d512,
            "512/224" => DigestSizes.d512t224,
            "512/256" => DigestSizes.d512t256,
                
            _ => DigestSizes.NONE
        };
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
