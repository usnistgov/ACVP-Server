using System.IO;
using System.Linq;
using LCAVPCore.AlgorithmResults.TDES;

namespace LCAVPCore.AlgorithmResultsExtractors.TDES
{
	public class TDES_CMAC_ResultsExtractor : IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "CMAC_Summary.txt";
		private string _summaryFilePath;

		public TDES_CMAC_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public TDES_CMAC_Results Extract()
		{
			TDES_CMAC_Results results = new TDES_CMAC_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Read the summary file content
			string[] lines = File.ReadAllLines(_summaryFilePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

			results.KeySize2GeneratePassed = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "[Alg = TDES KeySize = 2 Mode = Generate]") == "Passed";
			results.KeySize2VerifyPassed = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "[Alg = TDES KeySize = 2 Mode = Verify]") == "Passed";
			results.KeySize3GeneratePassed = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "[Alg = TDES KeySize = 3 Mode = Generate]") == "Passed";
			results.KeySize3VerifyPassed = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "[Alg = TDES KeySize = 3 Mode = Verify]") == "Passed";

			return results;
		}
	}
}