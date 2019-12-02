namespace ACVPCore.Providers
{
	public interface IVectorSetProvider
	{
		void Cancel(long id);
		void UpdateSubmittedResults(long vectorSetID, string results);
		void Insert(long vectorSetID, long testSessionID, string generatorVersion, long algorithmID);
	}
}