namespace ACVPCore.Providers
{
	public interface ITestSessionProvider
	{
		void Cancel(long id);
		void CancelVectorSets(long id);

		void Insert(long testSessionId, int acvVersionID, string generator, bool isSample, bool publishable, long userID);
	}
}