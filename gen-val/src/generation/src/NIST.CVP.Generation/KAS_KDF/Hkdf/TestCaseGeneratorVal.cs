using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Cr1;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.KAS_KDF.Hkdf
{
	public class TestCaseGeneratorVal : ITestCaseGeneratorAsync<TestGroup, TestCase>
	{
		private readonly IOracle _oracle;
		private readonly ITestCaseExpectationProvider<KasKdfTestCaseDisposition> _testCaseExpectationProvider;

		public TestCaseGeneratorVal(IOracle oracle, ITestCaseExpectationProvider<KasKdfTestCaseDisposition> testCaseExpectationProvider, int numberOfTestCasesToGenerate)
		{
			_oracle = oracle;
			_testCaseExpectationProvider = testCaseExpectationProvider;
			NumberOfTestCasesToGenerate = numberOfTestCasesToGenerate;			
		}

		public int NumberOfTestCasesToGenerate { get; }
		public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup @group, bool isSample, int caseNo = -1)
		{
			var disposition = _testCaseExpectationProvider.GetRandomReason();

			try
			{
				var result = await _oracle.GetKasKdfValHkdfTestAsync(new KasKdfValHkdfParameters()
				{
					Disposition = disposition.GetReason(),
					KdfConfiguration = group.KdfConfiguration,
					ZLength = group.ZLength
				});
				
				return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase()
				{
					KdfParameter = result.KdfInputs,
					FixedInfoPartyU = result.FixedInfoPartyU,
					FixedInfoPartyV = result.FixedInfoPartyV,
					Dkm = result.DerivedKeyingMaterial,
					TestPassed = result.TestPassed
				});
			}
			catch (Exception e)
			{
				Logger.Error(e);
				return new TestCaseGenerateResponse<TestGroup, TestCase>(e.Message);
			}
		}
		
		private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
	}
}