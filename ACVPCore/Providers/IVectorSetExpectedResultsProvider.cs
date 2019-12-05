namespace ACVPCore.Providers
{
	public interface IVectorSetExpectedResultsProvider
	{
		void InsertWithCapabilities(long vectorSetID, string capabilities);
	}
}