using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.DRBG.Enums;

namespace NIST.CVP.Crypto.DRBG.Helpers
{
    public class DrbgAttributes
    {
        public DrbgMechanism Mechanism { get; }
        public DrbgMode Mode { get; }

        public int MaxSecurityStrength { get; }

        public long MinEntropyInputLength { get; }
        public long MaxEntropyInputLength { get; }
        public long MaxPersonalizationStringLength { get; }
        public long MaxAdditionalInputStringLength { get; }
        public long MaxNumberOfBitsPerRequest { get; }
        public long MaxNumberOfRequestsBetweenReseeds { get; }
        public long MinNonceLength { get; }
        public long MaxNonceLength { get; }

        public string MechanismAsString => EnumHelpers.GetEnumDescriptionFromEnum(Mechanism);
        public string ModeAsString => EnumHelpers.GetEnumDescriptionFromEnum(Mode);

        public DrbgAttributes(DrbgMechanism mechanism, DrbgMode mode, int maxSecurityStrength, long minEntropyInputLength, long maxEntropyInputLength, long maxPersoStringLength, long maxAdditStringLength, long maxNumberOfBitsPerRequest, long maxNumberOfRequestsBetweenReseeds, long minNonceLength, long maxNonceLength)
        {
            Mechanism = mechanism;
            Mode = mode;

            MaxSecurityStrength = maxSecurityStrength;

            MinEntropyInputLength = minEntropyInputLength;
            MaxEntropyInputLength = maxEntropyInputLength;
            MaxPersonalizationStringLength = maxPersoStringLength;
            MaxAdditionalInputStringLength = maxAdditStringLength;
            MaxNumberOfBitsPerRequest = maxNumberOfBitsPerRequest;
            MaxNumberOfRequestsBetweenReseeds = maxNumberOfRequestsBetweenReseeds;
            MinNonceLength = minNonceLength;
            MaxNonceLength = maxNonceLength;
        }
    }
}
