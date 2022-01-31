namespace NIST.CVP.ACVTS.Libraries.Math.Entropy
{
    /// <summary>
    /// Means of getting entropy that has (a minimum) number of leading zeroes.
    /// </summary>
    public interface IEntropyProviderLeadingOnesFactory : IEntropyProviderFactory
    {
        /// <summary>
        /// The (minimum) number of leading ones to be returned by the <see cref="IEntropyProvider"/>.
        /// </summary>
        int MinimumLeadingOnes { get; set; }
    }
}
