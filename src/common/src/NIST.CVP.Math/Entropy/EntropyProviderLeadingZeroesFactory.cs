namespace NIST.CVP.Math.Entropy
{
    public class EntropyProviderLeadingZeroesFactory : IEntropyProviderLeadingZeroesFactory
    {
        public IEntropyProvider GetEntropyProvider(EntropyProviderTypes providerType)
        {
            throw new System.NotImplementedException();
        }

        public int MinimumLeadingZeroes { get; set; }
    }
}