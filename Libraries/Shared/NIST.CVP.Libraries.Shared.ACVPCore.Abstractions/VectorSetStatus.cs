using System.Runtime.Serialization;

namespace NIST.CVP.Libraries.Shared.ACVPCore.Abstractions
{
	public enum VectorSetStatus
	{
		[EnumMember(Value = "initial")]
		Initial = 0,
		[EnumMember(Value = "processed")]
		Processed = 1,
		[EnumMember(Value = "katReceieved")]
		KATReceived = 2,
		[EnumMember(Value = "passed")]
		Passed = 3,
		[EnumMember(Value = "failed")]
		Failed = 4,
		[EnumMember(Value = "cancelled")]
		Cancelled = 5,
		[EnumMember(Value = "resubmitAnswers")]
		ResubmitAnswers = 6,
		[EnumMember(Value = "error")]
		Error = -1
	}
}
