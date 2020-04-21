namespace NIST.CVP.Libraries.Internal.ACVPCore
{
	public enum TestSessionStatus
	{
		Unknown = 0,
		Cancelled = 1,
		PendingEvaluation = 2,
		Failed = 3,
		Passed = 4,
		SubmittedForApproval = 5,
		Published = 6,
		Expired = 7
	}
}
