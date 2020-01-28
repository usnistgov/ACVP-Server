namespace NIST.CVP.Common.Interfaces
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
    }
}