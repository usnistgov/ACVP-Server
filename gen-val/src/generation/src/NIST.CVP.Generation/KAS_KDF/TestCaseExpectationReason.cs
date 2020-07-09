using NIST.CVP.Common.Helpers;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS_KDF
{
	public class TestCaseExpectationReason : ITestCaseExpectationReason<KasKdfTestCaseDisposition>
	{
		private readonly KasKdfTestCaseDisposition _reason;

		public TestCaseExpectationReason(KasKdfTestCaseDisposition reason)
		{
			_reason = reason;
		}

		public string GetName()
		{
			return EnumHelpers.GetEnumDescriptionFromEnum(_reason);
		}

		public KasKdfTestCaseDisposition GetReason()
		{
			return _reason;
		}
	}
}