using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;
using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Shared.Results;


namespace NIST.CVP.Libraries.Internal.ACVPCore.Providers
{
	public interface ITestSessionProvider
	{
		Result CancelVectorSets(long id);
		Result Insert(long testSessionId, int acvVersionID, string generator, bool isSample, long userID);
		TestSessionStatus GetStatus(long testSessionID);
		Result UpdateStatus(long testSessionID, TestSessionStatus testSessionStatus);
		PagedEnumerable<TestSessionLite> Get(TestSessionListParameters param);
		TestSession Get(long testSessionId);
		List<VectorSet> GetVectorSetsForTestSession(long testSessionId);
		long GetTestSessionIDFromVectorSet(long vectorSetID);
		bool TestSessionExists(long testSessionID);
		void Expire(int ageInDays);
	}
}