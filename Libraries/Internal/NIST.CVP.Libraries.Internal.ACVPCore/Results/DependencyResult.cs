using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Results
{
	public class DependencyResult : InsertResult
	{
		public string URL { get => $"/admin/validations/dependencies/{ID}"; }

		public DependencyResult(long id) : base(id) { }

		public DependencyResult(string errorMessage) : base(errorMessage) { }
	}
}
