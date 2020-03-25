using System.IO;
using System.Linq;
using LCAVPCore.AlgorithmResults.AES;

namespace LCAVPCore.AlgorithmResultsExtractors.AES
{
	public class AES_KWP_ResultsExtractor : IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "KeyWrap38F_Summary.txt";
		private string _summaryFilePath;

		public AES_KWP_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public AES_KWP_Results Extract()
		{
			AES_KWP_Results results = new AES_KWP_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Read the summary file content - since we want only the lines starting with KWP-AE or KWP-AD, filter to that to get rid of the KW and TDES-KW lines
			string[] lines = File.ReadAllLines(_summaryFilePath).Where(l => l.StartsWith("KWP-A")).ToArray();

			results.Encrypt128Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "KWP-AE with AES-128 cipher function") == "PASSED";
			results.Encrypt192Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "KWP-AE with AES-192 cipher function") == "PASSED";
			results.Encrypt256Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "KWP-AE with AES-256 cipher function") == "PASSED";
			results.Decrypt128Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "KWP-AD with AES-128 cipher function") == "PASSED";
			results.Decrypt192Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "KWP-AD with AES-192 cipher function") == "PASSED";
			results.Decrypt256Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "KWP-AD with AES-256 cipher function") == "PASSED";
			results.Encrypt128InversePassed = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "KWP-AE with AES-128 inverse cipher function") == "PASSED";
			results.Encrypt192InversePassed = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "KWP-AE with AES-192 inverse cipher function") == "PASSED";
			results.Encrypt256InversePassed = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "KWP-AE with AES-256 inverse cipher function") == "PASSED";
			results.Decrypt128InversePassed = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "KWP-AD with AES-128 inverse cipher function") == "PASSED";
			results.Decrypt192InversePassed = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "KWP-AD with AES-192 inverse cipher function") == "PASSED";
			results.Decrypt256InversePassed = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "KWP-AD with AES-256 inverse cipher function") == "PASSED";

			return results;
		}
	}
}