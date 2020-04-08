using NIST.CVP.Results;

namespace ACVPCore.Results
{
	public class AcvpUserResult : InsertResult
	{
		public string URL { get => $"/admin/validations/persons/{ID}"; }

		public AcvpUserResult(long id) : base(id) { }

		public AcvpUserResult(string errorMessage) : base(errorMessage) { }
	}
}