using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Oracle
{
    public interface IOrleansClientClustering
    {
        /// <summary>
        /// Configure the <see cref="IClientBuilder"/> to use the appropriate type of clustering.
        /// </summary>
        /// <param name="builder">The <see cref="IClientBuilder"/> to manipulate.</param>
        void ConfigureClustering(IClientBuilder builder);
    }
}
