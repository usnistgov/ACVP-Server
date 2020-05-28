using NIST.CVP.Common.Helpers;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar3;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS_SSC.Sp800_56Ar3.TestCaseExpectations
{
	public class TestCaseExpecttionReason : ITestCaseExpectationReason<KasSscTestCaseExpectation>
	{
		private readonly KasSscTestCaseExpectation _reason;

		public TestCaseExpecttionReason(KasSscTestCaseExpectation reason)
		{
			_reason = reason;
		}

		public string GetName()
		{
			return EnumHelpers.GetEnumDescriptionFromEnum(_reason);
		}

		public KasSscTestCaseExpectation GetReason()
		{
			return _reason;
		}
	}
}