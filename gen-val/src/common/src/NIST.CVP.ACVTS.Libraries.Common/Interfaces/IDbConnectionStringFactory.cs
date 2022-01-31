namespace NIST.CVP.ACVTS.Libraries.Common.Interfaces
{
    /// <summary>
    /// Gets a connection string from a collection of connection strings.
    /// </summary>
    public interface IDbConnectionStringFactory
    {
        /// <summary>
        /// Get connection string matching name.
        /// </summary>
        /// <param name="connectionStringName">The connection string label to get.</param>
        /// <returns></returns>
        string GetConnectionString(string connectionStringName);
        /// <summary>
        /// Get connection string for use with Mighty.
        /// </summary>
        /// <param name="connectionStringName">The name of the connection string to use.</param>
        /// <returns></returns>
        string GetMightyConnectionString(string connectionStringName);
    }
}
