namespace NIST.CVP.Libraries.Shared.ACVPCore.Abstractions
{
	public enum VectorSetStatus
	{
		Initial = 0,
		Processed = 1,
		KATReceived = 2,
		Passed = 3,
		Failed = 4,
		Cancelled = 5,
		ResubmitAnswers = 6,
		Error = -1
	}
}
