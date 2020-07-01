using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KAS_SSC.Sp800_56Ar3.TestCaseExpectations;

namespace NIST.CVP.Generation.KAS_KDF
{
	public class TestCaseExpectationProvider : ITestCaseExpectationProvider<KasKdfTestCaseDisposition>
	{
		private readonly ConcurrentQueue<TestCaseExpectationReason> _expectationReasons;

		public int ExpectationCount => _expectationReasons.Count;
		
		public TestCaseExpectationProvider(bool isSample)
		{
			var expectationReasons = new List<TestCaseExpectationReason>();

			if (isSample)
			{
				var totalTests = 5;
				
				expectationReasons.Add(new TestCaseExpectationReason(KasKdfTestCaseDisposition.Fail));
				expectationReasons.Add(new TestCaseExpectationReason(KasKdfTestCaseDisposition.SuccessLeadingZeroNibble));
				
				expectationReasons.Add(new TestCaseExpectationReason(KasKdfTestCaseDisposition.Success), totalTests - expectationReasons.Count);
			}
			else
			{
				var totalTests = 15;

				expectationReasons.Add(new TestCaseExpectationReason(KasKdfTestCaseDisposition.Fail), 2);
				expectationReasons.Add(new TestCaseExpectationReason(KasKdfTestCaseDisposition.SuccessLeadingZeroNibble), 2);

				expectationReasons.Add(new TestCaseExpectationReason(KasKdfTestCaseDisposition.Success), totalTests - expectationReasons.Count);
			}
			
			_expectationReasons = new ConcurrentQueue<TestCaseExpectationReason>(expectationReasons.Shuffle());
		}
		
		public ITestCaseExpectationReason<KasKdfTestCaseDisposition> GetRandomReason()
		{
			if (_expectationReasons.TryDequeue(out var reason))
			{
				return reason;
			}
            
			throw new IndexOutOfRangeException($"No {nameof(_expectationReasons)} remaining to pull");
		}
	}
}