using System;

namespace NIST.CVP.Math.Entropy
{
    /// <summary>
    /// Used for retrieving an instance of an <see cref="IEntropyProvider"/>
    /// </summary>
    public class EntropyProviderFactory : IEntropyProviderFactory
    {
        /// <summary>
        /// Returns a new instance of an <see cref="IEntropyProvider"/>
        /// </summary>
        /// <param name="providerType">The <see cref="IEntropyProvider"/> type </param>
        /// <exception cref="ArgumentException">Thrown when <see cref="providerType"/> is invalid</exception>
        /// <returns></returns>
        public IEntropyProvider GetEntropyProvider(EntropyProviderTypes providerType)
        {
            switch (providerType)
            {
                case EntropyProviderTypes.Testable:
                    return new TestableEntropyProvider();
                case EntropyProviderTypes.Random:
                    return new EntropyProvider(new Random800_90());
                default:
                    throw new ArgumentException($"Invalid {providerType} supplied.");
            }
        }
    }
}
