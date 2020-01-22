using System.Collections.Generic;
using ACVPCore.Models;
using ACVPCore.Providers;
using ACVPCore.Results;

namespace ACVPCore.Services
{
	public class TestSessionService : ITestSessionService
	{
		private readonly ITestSessionProvider _testSessionProvider;

		public TestSessionService(ITestSessionProvider testSessionProvider)
		{
			_testSessionProvider = testSessionProvider;
		}

		public Result Cancel(long testSessionID)
		{
			//Cancel all the vector sets on the test session
			Result result = _testSessionProvider.CancelVectorSets(testSessionID);

			if (!result.IsSuccess)
			{
				return result;
			}

			//Cancel the test session
			result = _testSessionProvider.Cancel(testSessionID);

			return result;
		}

		public Result Create(long testSessionId, string acvVersion, string generator, bool isSample, long userID)
		{
			int acvVersionID = acvVersion switch
			{
				"1.0" => 4,
				_ => -1
			};

			if (acvVersionID == -1)
			{
				return new Result("Unsupported ACVP version for Test Session creation");
			}

			return _testSessionProvider.Insert(testSessionId, acvVersionID, generator, isSample, !isSample, userID);
		}

		public bool CanSubmitForApproval(long testSessionID) => _testSessionProvider.GetStatus(testSessionID) == TestSessionStatus.Passed;

		public TestSessionStatus GetStatus(long testSessionID) => _testSessionProvider.GetStatus(testSessionID);

		public Result UpdateStatus(long testSessionID, TestSessionStatus testSessionStatus) => _testSessionProvider.UpdateStatus(testSessionID, testSessionStatus);
		public List<TestSessionLite> Get() => _testSessionProvider.Get();
		public TestSession Get(long testSessionId)
		{
			var testSession = _testSessionProvider.Get(testSessionId);
			testSession.VectorSets = _testSessionProvider.GetVectorSetsForTestSession(testSessionId);
			return testSession;
		}
	}
}
