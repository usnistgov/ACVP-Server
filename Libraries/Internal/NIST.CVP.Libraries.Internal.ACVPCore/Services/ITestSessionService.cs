using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Internal.ACVPCore.Models;
using NIST.CVP.Libraries.Internal.ACVPCore.Models.Parameters;
using NIST.CVP.Libraries.Shared.Results;


namespace NIST.CVP.Libraries.Internal.ACVPCore.Services
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
		long GetTestSessionIDFromVectorSet(long vectorSetID);
		bool TestSessionExists(long testSessionID);
		void Expire(int ageInDays);
	}
}