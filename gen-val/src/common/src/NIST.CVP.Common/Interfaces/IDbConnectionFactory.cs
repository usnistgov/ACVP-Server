using System.Data.Common;

namespace NIST.CVP.Common.Interfaces
{
    public interface IDbConnectionFactory
    {
        DbConnection Get(string connectionString);
    }
}