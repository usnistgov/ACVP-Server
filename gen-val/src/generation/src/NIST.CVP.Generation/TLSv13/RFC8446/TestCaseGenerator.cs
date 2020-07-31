using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.TLSv13.RFC8446
{
	public class TestCaseGenerator : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
	{
		private readonly IOracle _oracle;

		public int NumberOfTestCasesToGenerate { get; private set; } = 25;

		public TestCaseGenerator(IOracle oracle)
		{
			_oracle = oracle;
		}

		public GenerateResponse PrepareGenerator(TestGroup @group, bool isSample)
		{
			if (isSample)
			{
				NumberOfTestCasesToGenerate = 5;
			}
			return new GenerateResponse();
		}
        
		public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
		{
			var param = new TlsKdfv13Parameters
			{
				HashAlg = group.HmacAlg,
				RandomLength = group.RandomLength
			};

			try
			{
				var result = await _oracle.GetTlsv13CaseAsync(param);

				var testCase = new TestCase
				{
					Dhe = result.Dhe,
					Psk = result.Dhe,
					
					HelloClientRandom = result.HelloClientRandom,
					HelloServerRandom = result.HelloServerRandom,
					
					FinishedClientRandom = result.FinishClientRandom,
					FinishedServerRandom = result.FinishServerRandom,
					
					ExporterMasterSecret = result.DerivedKeyingMaterial.MasterSecretResult.ExporterMasterSecret,
				};

				return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
			}
			catch (Exception ex)
			{
				ThisLogger.Error(ex);
				return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
			}
		}

		public ILogger ThisLogger => LogManager.GetCurrentClassLogger();
	}
}