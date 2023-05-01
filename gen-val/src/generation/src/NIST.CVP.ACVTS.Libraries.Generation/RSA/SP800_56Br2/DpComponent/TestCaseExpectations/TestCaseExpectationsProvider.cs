using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.Sp800_56Br2.DpComponent.TestCaseExpectations
{
    public class TestCaseExpectationProvider : ITestCaseExpectationProvider<RsaDpDisposition>
    {
        private readonly ConcurrentQueue<TestCaseExpectationReason> _expectationReasons;

        public int ExpectationCount { get; set; }
        private int NumberOfTestCasesToGenerate { get; set; } = 15;
        
        public TestCaseExpectationProvider(bool isSample = false)
        {
            List<TestCaseExpectationReason> expectationReasons = new List<TestCaseExpectationReason>();
            
            if (isSample)
            {
                NumberOfTestCasesToGenerate -= 5;
            }

            // We always want 4 failing test cases per group, randomized throughout the test case list
            expectationReasons.Add(new TestCaseExpectationReason(RsaDpDisposition.CtEqual0));
            expectationReasons.Add(new TestCaseExpectationReason(RsaDpDisposition.CtEqual1));
            expectationReasons.Add(new TestCaseExpectationReason(RsaDpDisposition.CtEqualNMinusOne));
            expectationReasons.Add(new TestCaseExpectationReason(RsaDpDisposition.CtGreaterNMinusOne));
            
            int i = 4;
            // Add the remaining passing cases
            while (i++ < NumberOfTestCasesToGenerate)
            {
                expectationReasons.Add(new TestCaseExpectationReason(RsaDpDisposition.None));
            }
            
            // Randomize list and stuff it into the Queue for easier iteration/bounds checking
            _expectationReasons = new ConcurrentQueue<TestCaseExpectationReason>(expectationReasons.Shuffle());
            ExpectationCount = _expectationReasons.Count;
        }
        
        // Is random since it's shuffled in the constructor
        public ITestCaseExpectationReason<RsaDpDisposition> GetRandomReason()
        {
            if (_expectationReasons.TryDequeue(out var reason))
            {
                return reason;
            }

            throw new IndexOutOfRangeException($"No {nameof(_expectationReasons)} remaining to pull");
        }
    }
}
