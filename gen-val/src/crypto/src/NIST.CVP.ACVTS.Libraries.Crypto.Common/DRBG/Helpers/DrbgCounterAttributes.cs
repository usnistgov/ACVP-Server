using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG.Helpers
{
    public class DrbgCounterAttributes
    {
        public DrbgMechanism Mechanism { get; }
        public DrbgMode Mode { get; }
        public int MaxSecurityStrength { get; }
        public int BlockSize { get; }
        public int OutputLength { get; }
        public int KeyLength { get; }

        public int SeedLength => OutputLength + KeyLength;
        public string MechanismAsString => EnumHelpers.GetEnumDescriptionFromEnum(Mechanism);
        public string ModeAsString => EnumHelpers.GetEnumDescriptionFromEnum(Mode);

        public DrbgCounterAttributes(DrbgMechanism mechanism, DrbgMode mode, int maxSecurityStrength, int blockSize, int outputLength, int keyLength)
        {
            Mechanism = mechanism;
            Mode = mode;
            MaxSecurityStrength = maxSecurityStrength;
            BlockSize = blockSize;
            OutputLength = outputLength;
            KeyLength = keyLength;
        }
    }
}
