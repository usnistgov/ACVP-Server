using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Hash.TupleHash;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.TupleHash
{
    public class TestCaseValidatorMCTHash : ITestCaseValidatorAsync<TestGroup, TestCase>
    {
        private readonly TestCase _expectedResult;

        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidatorMCTHash(TestCase expectedResult)
        {
            _expectedResult = expectedResult;
        }

        public Task<TestCaseValidation> ValidateAsync(TestCase suppliedResult, bool showExpected = false)
        {
            var errors = new List<string>();
            var expected = new Dictionary<string, string>();
            var provided = new Dictionary<string, string>();

            ValidateArrayResultPresent(suppliedResult, errors);
            if (errors.Count == 0)
            {
                CheckResults(suppliedResult, errors, expected, provided);
            }

            if (errors.Count > 0)
            {
                return Task.FromResult(new TestCaseValidation 
                { 
                    TestCaseId = suppliedResult.TestCaseId, 
                    Result = Core.Enums.Disposition.Failed, 
                    Reason = string.Join("; ", errors),
                    Expected = expected.Count != 0 && showExpected ? expected : null,
                    Provided = provided.Count != 0 && showExpected ? provided : null
                });
            }

            return Task.FromResult(new TestCaseValidation
            {
                TestCaseId = suppliedResult.TestCaseId,
                Result = Core.Enums.Disposition.Passed
            });
        }

        private void ValidateArrayResultPresent(TestCase suppliedResult, List<string> errors)
        {
            if (suppliedResult.ResultsArray == null || suppliedResult.ResultsArray.Count == 0)
            {
                errors.Add($"{nameof(suppliedResult.ResultsArray)} was not present in the {nameof(TestCase)}");
                return;
            }

            if (suppliedResult.ResultsArray.Any(a => a.Tuple == null))
            {
                errors.Add($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponse.Tuple)}");
            }
            if (suppliedResult.ResultsArray.Any(a => a.Digest == null))
            {
                errors.Add($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponse.Digest)}");
            }
            if (suppliedResult.ResultsArray.Any(a => a.Customization == null))
            {
                errors.Add($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponse.Customization)}");
            }
        }

        private void CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            for (var i = 0; i < _expectedResult.ResultsArray.Count; i++)
            {
                if (!TuplesEqual(_expectedResult.ResultsArray[i].Tuple, suppliedResult.ResultsArray[i].Tuple))
                {
                    errors.Add($"Tuple does not match on iteration {i}");
                    expected.Add($"Tuple {i}", TupleToString(_expectedResult.ResultsArray[i].Tuple));
                    provided.Add($"Tuple {i}", TupleToString(suppliedResult.ResultsArray[i].Tuple));
                }
                if (!_expectedResult.ResultsArray[i].Digest.Equals(suppliedResult.ResultsArray[i].Digest))
                {
                    errors.Add($"Digest does not match on iteration {i}");
                    expected.Add($"Digest {i}", _expectedResult.ResultsArray[i].Digest.ToHex());
                    provided.Add($"Digest {i}", suppliedResult.ResultsArray[i].Digest.ToHex());
                }
                if (!_expectedResult.ResultsArray[i].Customization.Equals(suppliedResult.ResultsArray[i].Customization))
                {
                    errors.Add($"Customization does not match on iteration {i}");
                    expected.Add($"Customization {i}", _expectedResult.ResultsArray[i].Customization);
                    provided.Add($"Customization {i}", suppliedResult.ResultsArray[i].Customization);
                }
            }
        }

        private string TupleToString(IEnumerable<BitString> tuple)
        {
            var output = "";
            foreach (var element in tuple)
            {
                output += element.ToHex() + "\n";
            }
            return output;
        }

        private bool TuplesEqual(IEnumerable<BitString> tuple1, IEnumerable<BitString> tuple2)
        {
            if (tuple1.Count() != tuple2.Count())
            {
                return false;
            }

            for (int i = 0; i < tuple1.Count(); i++)
            {
                if (!tuple1.ElementAt(i).Equals(tuple2.ElementAt(i)))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
