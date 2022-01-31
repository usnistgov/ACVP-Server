namespace NIST.CVP.ACVTS.Libraries.Math.Entropy
{
    public class EntropyProviderLeadingOnesFactory : IEntropyProviderLeadingOnesFactory
    {
        private readonly IRandom800_90 _random;

        public EntropyProviderLeadingOnesFactory(IRandom800_90 random)
        {
            _random = random;
        }

        public IEntropyProvider GetEntropyProvider(EntropyProviderTypes providerType)
        {
            return new EntropyProviderLeadingOnes(_random, MinimumLeadingOnes);
        }

        public int MinimumLeadingOnes { get; set; }
    }
}
