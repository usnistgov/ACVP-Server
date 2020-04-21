using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Results
{
	public class AcvpUserResult : InsertResult
	{
		public string URL { get => $"/admin/validations/persons/{ID}"; }

		public AcvpUserResult(long id) : base(id) { }

		public AcvpUserResult(string errorMessage) : base(errorMessage) { }
	}
}