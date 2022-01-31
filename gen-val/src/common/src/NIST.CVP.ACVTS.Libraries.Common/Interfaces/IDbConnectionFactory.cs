using System.Data.Common;

namespace NIST.CVP.ACVTS.Libraries.Common.Interfaces
{
    public interface IDbConnectionFactory
    {
        DbConnection Get(string connectionString);
    }
}
