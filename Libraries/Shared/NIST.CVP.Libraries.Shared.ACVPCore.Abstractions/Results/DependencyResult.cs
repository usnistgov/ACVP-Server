using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Results
{
	public class DependencyResult : InsertResult
	{
		public string URL { get => $"/admin/validations/dependencies/{ID}"; }

		public DependencyResult(long id) : base(id) { }

		public DependencyResult(string errorMessage) : base(errorMessage) { }
	}
}
