﻿using ACVPCore.Results;

namespace ACVPCore.Providers
{
	public interface ITestSessionProvider
	{
		Result Cancel(long id);
		Result CancelVectorSets(long id);
		Result Insert(long testSessionId, int acvVersionID, string generator, bool isSample, bool publishable, long userID);
		TestSessionStatus GetStatus(long testSessionID);
		Result UpdateStatus(long testSessionID, TestSessionStatus testSessionStatus);
	}
}