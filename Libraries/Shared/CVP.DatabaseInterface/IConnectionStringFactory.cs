namespace CVP.DatabaseInterface
{
	public interface IConnectionStringFactory
	{
		string GetConnectionString(string connectionStringName);
		string GetMightyConnectionString(string connectionStringName);
	}
}