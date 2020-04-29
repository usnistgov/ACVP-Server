namespace NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Results
{
	public class KillThisBaseResult
	{
		public string ErrorMessage { get; set; }
		public bool IsSuccess { get => string.IsNullOrEmpty(ErrorMessage); }
	}
}
