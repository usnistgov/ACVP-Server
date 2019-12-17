namespace ACVPCore.Results
{
	public class ImplementationResult : InsertResult
	{
		public string URL { get => $"/admin/validations/modules/{ID}"; }

		public ImplementationResult(long id) : base(id) { }

		public ImplementationResult(string errorMessage) : base(errorMessage) { }
	}
}
