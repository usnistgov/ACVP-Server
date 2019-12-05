using System.Data;

namespace NIST.CVP.Common.Interfaces
{
    public interface IDbConnectionFactory
    {
        IDbConnection Get(string connectionString);
    }
}