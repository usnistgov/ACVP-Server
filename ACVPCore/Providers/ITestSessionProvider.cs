using System.Collections.Generic;
using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;

namespace ACVPCore.Providers
{
	public interface ITestSessionProvider
	{
		Result Cancel(long id);
		Result CancelVectorSets(long id);
		Result Insert(long testSessionId, int acvVersionID, string generator, bool isSample, bool publishable, long userID);
		TestSessionStatus GetStatus(long testSessionID);
		Result UpdateStatus(long testSessionID, TestSessionStatus testSessionStatus);
		PagedEnumerable<TestSessionLite> Get(TestSessionListParameters param);
		TestSession Get(long testSessionId);
		List<VectorSet> GetVectorSetsForTestSession(long testSessionId);
		long GetTestSessionIDFromVectorSet(long vectorSetID);
		bool TestSessionExists(long testSessionID);
	}
}