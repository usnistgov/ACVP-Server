namespace NIST.CVP.Results
{
	public class Result
	{
		public string ErrorMessage { get; set; }
		public bool IsSuccess { get => string.IsNullOrEmpty(ErrorMessage); }

		public Result() { }
		public Result(string errorMessage)
		{
			ErrorMessage = errorMessage;
		}
	}
}
