using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Results
{
	public class OEResult : InsertResult
	{
		public string URL { get => $"/admin/validations/oes/{ID}"; }

		public OEResult(long id) : base(id) { }

		public OEResult(string errorMessage) : base(errorMessage) { }
	}
}
