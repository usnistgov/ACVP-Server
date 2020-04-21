namespace NIST.CVP.Libraries.Shared.ACVPCore.Abstractions
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
