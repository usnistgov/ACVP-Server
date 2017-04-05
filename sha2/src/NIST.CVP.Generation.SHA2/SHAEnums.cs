using System;

namespace NIST.CVP.Generation.SHA2
{
    public enum ModeValues
    {
        SHA1,
        SHA2
    }

    public enum DigestSizes
    {
        d160,
        d224,
        d256,
        d384,
        d512,
        d512t224,
        d512t256
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
                    throw new Exception("Bad mode for SHA.");
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
    }
}