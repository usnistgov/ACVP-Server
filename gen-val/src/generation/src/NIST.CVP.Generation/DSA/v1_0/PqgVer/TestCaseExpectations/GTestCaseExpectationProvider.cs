﻿using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Generation.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace NIST.CVP.Generation.DSA.v1_0.PqgVer.TestCaseExpectations
{
    public class GTestCaseExpectationProvider : ITestCaseExpectationProvider<DsaGDisposition>
    {
        private readonly ConcurrentQueue<GTestCaseExpectationReason> _expectationReasons;

        public GTestCaseExpectationProvider(bool isSample = false)
        {
            var expectationReasons = new List<GTestCaseExpectationReason>();

            // For a sample case, we always want a 'None' and a 'Modify G'
            if (isSample)
            {
                expectationReasons.Add(new GTestCaseExpectationReason(DsaGDisposition.None));
                expectationReasons.Add(new GTestCaseExpectationReason(DsaGDisposition.ModifyG));
            }
            // Otherwise we want everything
            else
            {
                expectationReasons.Add(new GTestCaseExpectationReason(DsaGDisposition.None), 2);
                expectationReasons.Add(new GTestCaseExpectationReason(DsaGDisposition.ModifyG), 3);
            }

            _expectationReasons = new ConcurrentQueue<GTestCaseExpectationReason>(expectationReasons.OrderBy(a => Guid.NewGuid()).ToList());
        }

        public ITestCaseExpectationReason<DsaGDisposition> GetRandomReason()
        {
            if (_expectationReasons.TryDequeue(out var reason))
            {
                return reason;
            }
            
            throw new IndexOutOfRangeException($"No {nameof(_expectationReasons)} remaining to pull");
        }
    }
}
