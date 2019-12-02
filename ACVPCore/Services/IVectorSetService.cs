namespace ACVPCore.Services
{
	public interface IVectorSetService
	{
		void Cancel(long vectorSetID);
		void UpdateSubmittedResults(long vectorSetID, string results);
		void Create(long vectorSetID, long testSessionID, string generatorVersion, long algorithmID, string capabilities);
	}
}