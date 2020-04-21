using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Results
{
	public class OrganizationResult : InsertResult
	{
		public string URL { get => $"/admin/validations/organizations/{ID}"; }

		public OrganizationResult(long id) : base(id) { }

		public OrganizationResult(string errorMessage) : base(errorMessage) { }
	}
}
