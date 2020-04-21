using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Internal.ACVPCore.Models;
using NIST.CVP.Libraries.Internal.ACVPCore.Models.Parameters;
using NIST.CVP.Libraries.Internal.ACVPCore.Providers;
using NIST.CVP.Libraries.Shared.Results;


namespace NIST.CVP.Libraries.Internal.ACVPCore.Services
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

			if (result.IsSuccess)
			{
				//Cancel the test session
				result = UpdateStatus(testSessionID, TestSessionStatus.Cancelled);
			}

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

		public Result UpdateStatus(long testSessionID, TestSessionStatus testSessionStatus)
		{
			//During the public rewrite we can simplify to this
			// => _testSessionProvider.UpdateStatus(testSessionID, testSessionStatus);

			//But until then need to do some special things in some cases to make Java happy
			switch (testSessionStatus)
			{
				case TestSessionStatus.Passed:
					//Set disposition = true, passed_data = now
					_testSessionProvider.UpdateStatusFieldsForJava(testSessionID, DateTime.Now, true, null, null);
					break;

				case TestSessionStatus.Failed:
					//Set disposition = false
					_testSessionProvider.UpdateStatusFieldsForJava(testSessionID, null, false, null, null);
					break;

				case TestSessionStatus.Cancelled:
					//Set disposition = false, publishable = false
					_testSessionProvider.UpdateStatusFieldsForJava(testSessionID, null, false, false, null);
					break;

				case TestSessionStatus.SubmittedForApproval:
					//Set published = true. Yes, that is dumb
					_testSessionProvider.UpdateStatusFieldsForJava(testSessionID, null, null, null, true);
					break;
			}
			
			//Finally do the basic update
			return _testSessionProvider.UpdateStatus(testSessionID, testSessionStatus);
		}

		public Result UpdateStatusFromVectorSets(long testSessionID)
		{
			//First check that it isn't already submitted or published, as don't want to change it then
			TestSessionStatus currentStatus = GetStatus(testSessionID);

			if (currentStatus == TestSessionStatus.SubmittedForApproval || currentStatus == TestSessionStatus.Published)
			{
				return new Result();
			}

			//Get what the new status should be
			TestSessionStatus newStatus = DetermineTestSessionStatusFromVectorSets(testSessionID);

			//Update the status if it changed (might as well avoid the update if not needed, and we already have the current status handy)
			return (newStatus == currentStatus) ? new Result() : UpdateStatus(testSessionID, newStatus);
		}

		public Result UpdateStatusFromVectorSetsWithVectorSetID(long vectorSetID)
		{
			long testSessionID = _testSessionProvider.GetTestSessionIDFromVectorSet(vectorSetID);

			if (testSessionID == -1)
			{
				return new Result($"Unable to find vector set {vectorSetID}");
			}
			else
			{
				return UpdateStatusFromVectorSets(testSessionID);
			}
		}

		private TestSessionStatus DetermineTestSessionStatusFromVectorSets(long testSessionID)
		{
			//Get the vector set statuses
			IEnumerable<VectorSetStatus> vectorSetStatuses = _testSessionProvider.GetVectorSetsForTestSession(testSessionID).Select(x => x.Status);

			if (vectorSetStatuses.All(x => x == VectorSetStatus.Passed)) return TestSessionStatus.Passed;																															//All Passed -> Passed
			if (vectorSetStatuses.All(x => x == VectorSetStatus.Cancelled)) return TestSessionStatus.Cancelled;																														//All Cancelled -> Cancelled
			if (vectorSetStatuses.Any(x => x == VectorSetStatus.Failed)) return TestSessionStatus.Failed;																															//Any Failed -> Failed
			if (vectorSetStatuses.All(x => x == VectorSetStatus.Passed || x == VectorSetStatus.Cancelled)) return TestSessionStatus.Passed;																							//All Passed or Canceled -> Passed - This comes after the All Passed and All Cancelled checks, as all cancelled would satisfy this, but we don't want it to get this far
			if (vectorSetStatuses.Any(x => x == VectorSetStatus.Initial || x == VectorSetStatus.Processed || x == VectorSetStatus.KATReceived || x == VectorSetStatus.Error)) return TestSessionStatus.PendingEvaluation;           //Anything isn't in a terminal status -> Pending Evaluation

			return TestSessionStatus.Unknown;
		}

		public PagedEnumerable<TestSessionLite> Get(TestSessionListParameters param) => _testSessionProvider.Get(param);
		public TestSession Get(long testSessionId)
		{
			var testSession = _testSessionProvider.Get(testSessionId);

			if (testSession == null)
				return null;

			testSession.VectorSets = _testSessionProvider.GetVectorSetsForTestSession(testSessionId);
			return testSession;
		}

		public long GetTestSessionIDFromVectorSet(long vectorSetID) => _testSessionProvider.GetTestSessionIDFromVectorSet(vectorSetID);

		public bool TestSessionExists(long testSessionID) => _testSessionProvider.TestSessionExists(testSessionID);

		public void Expire(int ageInDays) => _testSessionProvider.Expire(ageInDays);
	}
}
