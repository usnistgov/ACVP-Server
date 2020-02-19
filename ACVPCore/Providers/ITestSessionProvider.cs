using System.Collections.Generic;
using ACVPCore.Models;
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
		List<TestSessionLite> Get();
		TestSession Get(long testSessionId);
		List<TestVectorSetLite> GetVectorSetsForTestSession(long testSessionId);
		bool TestSessionExists(long testSessionID);
	}
}