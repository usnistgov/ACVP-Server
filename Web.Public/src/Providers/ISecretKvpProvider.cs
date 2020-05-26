namespace Web.Public.Providers
{
	/// <summary>
	/// Interface provides means of retrieving secret key value pairs.
	/// </summary>
	public interface ISecretKvpProvider
	{
		string GetValueFromKey(string key);
	}
}