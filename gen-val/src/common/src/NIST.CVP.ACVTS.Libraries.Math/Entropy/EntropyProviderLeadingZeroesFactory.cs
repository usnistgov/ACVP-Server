namespace NIST.CVP.ACVTS.Libraries.Math.Entropy
{
    public class EntropyProviderLeadingZeroesFactory : IEntropyProviderLeadingZeroesFactory
    {
        private readonly IRandom800_90 _random;

        public EntropyProviderLeadingZeroesFactory(IRandom800_90 random)
        {
            _random = random;
        }

        public IEntropyProvider GetEntropyProvider(EntropyProviderTypes providerType)
        {
            return new EntropyProviderLeadingZeroes(_random, MinimumLeadingZeroes);
        }

        public int MinimumLeadingZeroes { get; set; }
    }
}
