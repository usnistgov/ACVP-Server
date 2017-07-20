using System;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.SHA2
{
    public enum ModeValues
    {
        SHA1,
        SHA2,
        NONE
    }

    public enum DigestSizes
    {
        d160,
        d224,
        d256,
        d384,
        d512,
        d512t224,
        d512t256,
        NONE
    }

    public static class SHAEnumHelpers
    {
        public static string ModeToString(ModeValues mode)
        {
            switch (mode)
            {
                case ModeValues.SHA1:
                    return "sha1";
                case ModeValues.SHA2:
                    return "sha2";
                default:
                    throw new Exception("Bad mode for SHA.");
            }
        }

        public static ModeValues StringToMode(string mode)
        {
            switch (mode.ToLower())
            {
                case "sha1":
                    return ModeValues.SHA1;
                case "sha2":
                    return ModeValues.SHA2;
                default:
                    return ModeValues.NONE;
            }
        }

        public static ModeValues DigestSizeToMode(string digestSize)
        {
            switch (StringToDigest(digestSize))
            {
                case DigestSizes.d160:
                    return ModeValues.SHA1;

                case DigestSizes.d224:
                case DigestSizes.d256:
                case DigestSizes.d384:
                case DigestSizes.d512:
                case DigestSizes.d512t224:
                case DigestSizes.d512t256:
                    return ModeValues.SHA2;

                default:
                    throw new Exception("Bad digest size for SHA");
            }
        }

        public static string DigestToString(DigestSizes digestSize)
        {
            switch (digestSize)
            {
                case DigestSizes.d160:
                    return "160";
                case DigestSizes.d224:
                    return "224";
                case DigestSizes.d256:
                    return "256";
                case DigestSizes.d384:
                    return "384";
                case DigestSizes.d512:
                    return "512";
                case DigestSizes.d512t224:
                    return "512/224";
                case DigestSizes.d512t256:
                    return "512/256";
                default:
                    throw new Exception("Bad digest size for SHA");
            }
        }

        public static DigestSizes StringToDigest(string digestSize)
        {
            switch (digestSize.ToLower())
            {
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

        public static int DigestSizeToInt(DigestSizes digestSize)
        {
            switch (digestSize)
            {
                case DigestSizes.d160:
                    return 160;
                case DigestSizes.d224:
                case DigestSizes.d512t224:
                    return 224;
                case DigestSizes.d256:
                case DigestSizes.d512t256:
                    return 256;
                case DigestSizes.d384:
                    return 384;
                case DigestSizes.d512:
                    return 512;
                default:
                    throw new Exception("Bad digest size for SHA");
            }
        }

        public static int DetermineBlockSize(DigestSizes digestSize)
        {
            switch (digestSize)
            {
                case DigestSizes.d160:
                case DigestSizes.d224:
                case DigestSizes.d256:
                    return 512;

                case DigestSizes.d384:
                case DigestSizes.d512:
                case DigestSizes.d512t224:
                case DigestSizes.d512t256:
                    return 1024;

                default:
                    throw new Exception("Invalid block size for SHA");
            }
        }

        public static HashFunction StringToHashFunction(string hf)
        {
            switch (hf.ToLower())
            {
                case "sha-224":
                case "sha224":
                    return new HashFunction { Mode = ModeValues.SHA2, DigestSize = DigestSizes.d224 };

                case "sha-256":
                case "sha256":
                    return new HashFunction { Mode = ModeValues.SHA2, DigestSize = DigestSizes.d256 };

                case "sha-384":
                case "sha384":
                    return new HashFunction { Mode = ModeValues.SHA2, DigestSize = DigestSizes.d384 };

                case "sha-512":
                case "sha512":
                    return new HashFunction { Mode = ModeValues.SHA2, DigestSize = DigestSizes.d512 };

                case "sha-512/224":
                case "sha512224":
                    return new HashFunction { Mode = ModeValues.SHA2, DigestSize = DigestSizes.d512t224 };

                case "sha-512/256":
                case "sha512256":
                    return new HashFunction { Mode = ModeValues.SHA2, DigestSize = DigestSizes.d512t256 };

                case "sha-1":
                case "sha1":
                    return new HashFunction { Mode = ModeValues.SHA1, DigestSize = DigestSizes.d160 };

                default:
                    return new HashFunction { Mode = ModeValues.NONE, DigestSize = DigestSizes.NONE };
            }
        }

        public static string HashFunctionToString(HashFunction hf)
        {
            if (hf.Mode == ModeValues.SHA1)
            {
                if (hf.DigestSize == DigestSizes.d160)
                {
                    return "SHA-1";
                }
                else
                {
                    return "";
                }
            }
            else
            {
                if (hf.DigestSize == DigestSizes.d224)
                {
                    return "SHA-224";
                }
                else if (hf.DigestSize == DigestSizes.d256)
                {
                    return "SHA-256";
                }
                else if (hf.DigestSize == DigestSizes.d384)
                {
                    return "SHA-384";
                }
                else if (hf.DigestSize == DigestSizes.d512)
                {
                    return "SHA-512";
                }
                else if (hf.DigestSize == DigestSizes.d512t224)
                {
                    return "SHA-512/224";
                }
                else if (hf.DigestSize == DigestSizes.d512t256)
                {
                    return "SHA-512/256";
                }
                else
                {
                    return "";
                }
            }
        }

        public static BitString HashFunctionToBits(HashFunction hf)
        {
            switch (hf.DigestSize)
            {
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
                    return new BitString("40");
                default:
                    throw new Exception("Bad digest size");
            }
        }
    }
}