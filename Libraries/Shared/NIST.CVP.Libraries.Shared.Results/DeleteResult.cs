namespace NIST.CVP.Libraries.Shared.Results
{
	public class DeleteResult : Result
	{
		public bool IsInUse { get; set; } = false;

		public DeleteResult(ErrorReason reason)
		{
			switch (reason)
			{
				case ErrorReason.IsInUse:
					IsInUse = true;
					ErrorMessage = "Cannot be deleted, item is in use";
					break;

				default:
					ErrorMessage = "Unknown error";
					break;
			}
		}

		public DeleteResult(string errorMessage) : base(errorMessage) { }

		public DeleteResult(Result result)
		{
			ErrorMessage = result.ErrorMessage;
		}

		public enum ErrorReason
		{
			IsInUse
		}
	}

	
}

