using ACVPCore.Providers;

namespace ACVPCore.Services
{
	public class TestSessionService : ITestSessionService
	{
		ITestSessionProvider _testSessionProvider;

		public TestSessionService(ITestSessionProvider testSessionProvider)
		{
			_testSessionProvider = testSessionProvider;
		}

		public void Cancel(long testSessionID)
		{
			//Cancel all the vector sets on the test session
			_testSessionProvider.CancelVectorSets(testSessionID);

			//Cancel the test session
			_testSessionProvider.Cancel(testSessionID);
		}

		public void Create(long testSessionId, string acvVersion, string generator, bool isSample, long userID)
		{
			int acvVersionID = acvVersion switch
			{
				"1.0" => 4,
				_ => -1
			};

			_testSessionProvider.Insert(testSessionId, acvVersionID, generator, isSample, !isSample, userID);
		}
	}
}
