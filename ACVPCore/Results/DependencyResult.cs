namespace ACVPCore.Results
{
	public class DependencyResult : InsertResult
	{
		public string URL { get => $"/admin/validations/dependencies/{ID}"; }

		public DependencyResult(long id) : base(id) { }

		public DependencyResult(string errorMessage) : base(errorMessage) { }
	}
}
