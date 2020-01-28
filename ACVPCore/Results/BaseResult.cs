namespace ACVPCore.Results
{
	public class BaseResult
	{
		public string ErrorMessage { get; set; }
		public bool IsSuccess { get => string.IsNullOrEmpty(ErrorMessage); }
	}
}
