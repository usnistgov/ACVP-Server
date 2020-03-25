using System;
using System.Collections.Generic;
using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using NIST.CVP.Enumerables;
using NIST.CVP.Results;


namespace ACVPCore.Providers
{
	public interface ITestSessionProvider
	{
		Result CancelVectorSets(long id);
		Result Insert(long testSessionId, int acvVersionID, string generator, bool isSample, bool publishable, long userID);
		TestSessionStatus GetStatus(long testSessionID);
		Result UpdateStatus(long testSessionID, TestSessionStatus testSessionStatus);
		Result UpdateStatusFieldsForJava(long testSessionID, DateTime? passedDate, bool? disposition, bool? publishable, bool? published);
		PagedEnumerable<TestSessionLite> Get(TestSessionListParameters param);
		TestSession Get(long testSessionId);
		List<VectorSet> GetVectorSetsForTestSession(long testSessionId);
		long GetTestSessionIDFromVectorSet(long vectorSetID);
		bool TestSessionExists(long testSessionID);
		void Expire(int ageInDays);
	}
}