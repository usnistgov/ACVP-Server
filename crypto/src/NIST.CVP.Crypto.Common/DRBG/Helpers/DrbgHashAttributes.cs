using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.DRBG.Enums;

namespace NIST.CVP.Crypto.Common.DRBG.Helpers
{
    public class DrbgHashAttributes
    {
        public DrbgMechanism Mechanism { get; }
        public DrbgMode Mode { get; }
        public int MaxSecurityStrength { get; }
        public int OutputLength { get; }
        public int SeedLength { get; }

        public string MechanismAsString => EnumHelpers.GetEnumDescriptionFromEnum(Mechanism);
        public string ModeAsString => EnumHelpers.GetEnumDescriptionFromEnum(Mode);

        public DrbgHashAttributes(DrbgMechanism mechanism, DrbgMode mode, int maxSecurityStrength, int outputLength, int seedLength)
        {
            Mechanism = mechanism;
            Mode = mode;
            MaxSecurityStrength = maxSecurityStrength;
            OutputLength = outputLength;
            SeedLength = seedLength;
        }
    }
}
