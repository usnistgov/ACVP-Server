﻿using NIST.CVP.Generation.Core.Enums;

namespace NIST.CVP.Generation.Core.Tests.Fakes
{
    public class FakeTestCaseValidator<TTestGroup, TTestCase> : ITestCaseValidator<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>
    {
        public int TestCaseId { get; set; }
        private readonly Disposition _result;
        public FakeTestCaseValidator(Disposition result)
        {
            _result = result;
        }

        public TestCaseValidation Validate(TTestCase suppliedResult, bool showExpected)
        {
            return new TestCaseValidation {TestCaseId = TestCaseId, Result = _result};
        }
    }
}