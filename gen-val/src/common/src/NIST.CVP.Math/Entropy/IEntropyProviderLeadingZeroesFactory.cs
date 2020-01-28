namespace NIST.CVP.Math.Entropy
{
    /// <summary>
    /// Means of getting entropy that has (a minimum) number of leading zeroes.
    /// </summary>
    public interface IEntropyProviderLeadingZeroesFactory : IEntropyProviderFactory
    {
        /// <summary>
        /// The (minimum) number of leading zeroes to be returned by the <see cref="IEntropyProvider"/>.
        /// </summary>
        int MinimumLeadingZeroes { get; set; }
    }
}