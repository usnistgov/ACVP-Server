using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.v2_0.SpComponent.TestCaseExpectations
{
    public class TestCaseExpectationProvider : ITestCaseExpectationProvider<RsaSpDisposition>
    {
        private readonly ConcurrentQueue<TestCaseExpectationReason> _expectationReasons;

        public int ExpectationCount { get; set; }
        private int NumberOfTestCasesToGenerate { get; set; } = 15;
        private int NumberOfTestCasesToFail { get; set; } = 2;
        
        public TestCaseExpectationProvider(bool isSample = false)
        {
            List<TestCaseExpectationReason> expectationReasons = new List<TestCaseExpectationReason>();
            
            // 10 test cases if sample
            if (isSample)
            {
                NumberOfTestCasesToGenerate -= 5;
            }

            // We always want 2 failing test cases per group, randomized throughout the test case list
            expectationReasons.Add(new TestCaseExpectationReason(RsaSpDisposition.MsgEqualN));
            expectationReasons.Add(new TestCaseExpectationReason(RsaSpDisposition.MsgGreaterNLessModulo));
            
            // Add the remaining passing cases
            int totalReasons = NumberOfTestCasesToGenerate - NumberOfTestCasesToFail;
            while (totalReasons-- > 0)
            {
                expectationReasons.Add(new TestCaseExpectationReason(RsaSpDisposition.None));
            }
            
            // Randomize list and stuff it into the Queue for easier iteration/bounds checking
            _expectationReasons = new ConcurrentQueue<TestCaseExpectationReason>(expectationReasons.Shuffle());
            ExpectationCount = _expectationReasons.Count;
        }
        
        // Is random since it's shuffled in the constructor
        public ITestCaseExpectationReason<RsaSpDisposition> GetRandomReason()
        {
            if (_expectationReasons.TryDequeue(out var reason))
            {
                return reason;
            }

            throw new IndexOutOfRangeException($"No {nameof(_expectationReasons)} remaining to pull");
        }
    }
}
