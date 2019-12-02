namespace ACVPCore.Services
{
	public interface ITestSessionService
	{
		void Cancel(long testSessionID);

		void Create(long testSessionId, string acvVersion, string generator, bool isSample, long userID);
	}
}