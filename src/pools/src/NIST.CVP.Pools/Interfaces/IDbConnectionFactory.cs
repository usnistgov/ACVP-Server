using System.Data;

namespace NIST.CVP.Pools.Interfaces
{
    public interface IDbConnectionFactory
    {
        IDbConnection Get(string connectionString);
    }
}