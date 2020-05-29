using System.IO;
using System.Linq;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults.AES;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.AES
{
	public class AES_CMAC_ResultsExtractor : IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "CMAC_Summary.txt";
		private string _summaryFilePath;

		public AES_CMAC_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public AES_CMAC_Results Extract()
		{
			AES_CMAC_Results results = new AES_CMAC_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Read the summary file content
			string[] lines = File.ReadAllLines(_summaryFilePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

			results.Generate128Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "[Alg = AES KeySize = 128 Mode = Generate]") == "Passed";
			results.Generate192Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "[Alg = AES KeySize = 192 Mode = Generate]") == "Passed";
			results.Generate256Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "[Alg = AES KeySize = 256 Mode = Generate]") == "Passed";
			results.Verify128Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "[Alg = AES KeySize = 128 Mode = Verify]") == "Passed";
			results.Verify192Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "[Alg = AES KeySize = 192 Mode = Verify]") == "Passed";
			results.Verify256Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "[Alg = AES KeySize = 256 Mode = Verify]") == "Passed";

			return results;
		}
	}
}