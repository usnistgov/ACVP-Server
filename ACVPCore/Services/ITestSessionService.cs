using System.Collections.Generic;
using ACVPCore.Models;
using ACVPCore.Results;

namespace ACVPCore.Services
{
	public interface ITestSessionService
	{
		Result Cancel(long testSessionID);

		Result Create(long testSessionId, string acvVersion, string generator, bool isSample, long userID);

		bool CanSubmitForApproval(long testSessionID);
		TestSessionStatus GetStatus(long testSessionID);
		Result UpdateStatus(long testSessionID, TestSessionStatus testSessionStatus);
		List<TestSessionLite> Get();
		TestSession Get(long testSessionId);
		bool TestSessionExists(long testSessionID);
	}
}