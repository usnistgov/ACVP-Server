namespace NIST.CVP.Libraries.Shared.DatabaseInterface
{
	public interface IConnectionStringFactory
	{
		string GetConnectionString(string connectionStringName);
		string GetMightyConnectionString(string connectionStringName);
	}
}