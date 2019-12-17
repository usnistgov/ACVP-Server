using ACVPCore.Results;

namespace ACVPCore.Providers
{
	public interface IVectorSetExpectedResultsProvider
	{
		Result InsertWithCapabilities(long vectorSetID, string capabilities);
	}
}