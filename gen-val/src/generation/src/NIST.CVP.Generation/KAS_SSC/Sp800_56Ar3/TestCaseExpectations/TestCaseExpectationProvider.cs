using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar3;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS_SSC.Sp800_56Ar3.TestCaseExpectations
{
	public class TestCaseExpectationProvider : ITestCaseExpectationProvider<KasSscTestCaseExpectation>
	{
		private readonly ConcurrentQueue<TestCaseExpecttionReason> _expectationReasons;
		
		public TestCaseExpectationProvider(bool isSample = false)
		{
			var expectationReasons = new List<TestCaseExpecttionReason>();

			if (isSample)
			{
				expectationReasons.Add(new TestCaseExpecttionReason(KasSscTestCaseExpectation.FailChangedZ));
				expectationReasons.Add(new TestCaseExpecttionReason(KasSscTestCaseExpectation.PassLeadingZeroNibble));
				expectationReasons.Add(new TestCaseExpecttionReason(KasSscTestCaseExpectation.Pass), 2);
			}
			else
			{
				expectationReasons.Add(new TestCaseExpecttionReason(KasSscTestCaseExpectation.FailChangedZ), 2);
				expectationReasons.Add(new TestCaseExpecttionReason(KasSscTestCaseExpectation.PassLeadingZeroNibble), 2);
				expectationReasons.Add(new TestCaseExpecttionReason(KasSscTestCaseExpectation.Pass), 10);
			}
			
			_expectationReasons = new ConcurrentQueue<TestCaseExpecttionReason>(expectationReasons.Shuffle());
		}
		
		public ITestCaseExpectationReason<KasSscTestCaseExpectation> GetRandomReason()
		{
			if (_expectationReasons.TryDequeue(out var reason))
			{
				return reason;
			}
            
			throw new IndexOutOfRangeException($"No {nameof(_expectationReasons)} remaining to pull");
		}
	}
}