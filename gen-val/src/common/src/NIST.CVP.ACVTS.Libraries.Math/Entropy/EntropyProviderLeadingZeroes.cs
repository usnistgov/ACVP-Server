using System;

namespace NIST.CVP.ACVTS.Libraries.Math.Entropy
{
    public class EntropyProviderLeadingZeroes : EntropyProvider
    {
        private readonly int _minimumLeadingZeroes;

        public EntropyProviderLeadingZeroes(IRandom800_90 random, int minimumLeadingZeroes) : base(random)
        {
            _minimumLeadingZeroes = minimumLeadingZeroes;
        }

        public override BitString GetEntropy(int numberOfBits)
        {
            var totalRandomBits = numberOfBits - _minimumLeadingZeroes;

            if (totalRandomBits < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(numberOfBits),
                    "Random number of bits to generate cannot be less than 0.");
            }

            return new BitString(_minimumLeadingZeroes)
                .ConcatenateBits(base.GetEntropy(totalRandomBits));
        }
    }
}
