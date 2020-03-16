using System.Collections.Generic;
using ACVPCore.Models;
using ACVPCore.Models.Parameters;
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
		Result UpdateStatusFromVectorSets(long testSessionID);
		Result UpdateStatusFromVectorSetsWithVectorSetID(long vectorSetID);
		PagedEnumerable<TestSessionLite> Get(TestSessionListParameters param);
		TestSession Get(long testSessionId);
		bool TestSessionExists(long testSessionID);
	}
}