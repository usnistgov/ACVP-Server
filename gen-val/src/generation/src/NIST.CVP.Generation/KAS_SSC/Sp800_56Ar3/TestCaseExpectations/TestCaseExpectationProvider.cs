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

		public int ExpectationCount => _expectationReasons.Count;
		
		public TestCaseExpectationProvider(bool isSample, bool includeFailureTests)
		{
			var expectationReasons = new List<TestCaseExpecttionReason>();

			if (isSample)
			{
				var totalTests = 5;
				
				if (includeFailureTests)
				{
					expectationReasons.Add(new TestCaseExpecttionReason(KasSscTestCaseExpectation.FailChangedZ));
					expectationReasons.Add(new TestCaseExpecttionReason(KasSscTestCaseExpectation.PassLeadingZeroNibble));
				}
				
				expectationReasons.Add(new TestCaseExpecttionReason(KasSscTestCaseExpectation.Pass), totalTests - expectationReasons.Count);
			}
			else
			{
				var totalTests = 15;

				if (includeFailureTests)
				{
					expectationReasons.Add(new TestCaseExpecttionReason(KasSscTestCaseExpectation.FailChangedZ), 5);
					expectationReasons.Add(new TestCaseExpecttionReason(KasSscTestCaseExpectation.PassLeadingZeroNibble), 1);
				}
				
				expectationReasons.Add(new TestCaseExpecttionReason(KasSscTestCaseExpectation.Pass), totalTests - expectationReasons.Count);
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