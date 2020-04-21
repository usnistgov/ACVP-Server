using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Results
{
	public class OEResult : InsertResult
	{
		public string URL { get => $"/admin/validations/oes/{ID}"; }

		public OEResult(long id) : base(id) { }

		public OEResult(string errorMessage) : base(errorMessage) { }
	}
}
