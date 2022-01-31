using System;
using System.Collections;

namespace NIST.CVP.ACVTS.Libraries.Math.Entropy
{
    public class EntropyProviderLeadingOnes : EntropyProvider
    {
        private readonly int _minimumLeadingOnes;

        public EntropyProviderLeadingOnes(IRandom800_90 random, int minimumLeadingOnes) : base(random)
        {
            _minimumLeadingOnes = minimumLeadingOnes;
        }

        public override BitString GetEntropy(int numberOfBits)
        {
            var totalRandomBits = numberOfBits - _minimumLeadingOnes;

            if (totalRandomBits < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(numberOfBits),
                    "Random number of bits to generate cannot be less than 0.");
            }

            var bits = new BitArray(_minimumLeadingOnes, true);

            return new BitString(bits)
                .ConcatenateBits(base.GetEntropy(totalRandomBits));
        }
    }
}
