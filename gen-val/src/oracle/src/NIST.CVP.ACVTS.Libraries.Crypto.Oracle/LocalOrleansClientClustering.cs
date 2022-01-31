using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Oracle
{
    public class LocalOrleansClientClustering : IOrleansClientClustering
    {
        public void ConfigureClustering(IClientBuilder builder)
        {
            builder.UseLocalhostClustering();
        }
    }
}
