using System.IO;
using System.Linq;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults.AES;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.AES
{
	public class AES_KW_ResultsExtractor : IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "KeyWrap38F_Summary.txt";
		private string _summaryFilePath;

		public AES_KW_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public AES_KW_Results Extract()
		{
			AES_KW_Results results = new AES_KW_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Read the summary file content - since we want only the lines starting with KW-AE or KW-AD, filter to that to get rid of the KWP and TDES-KW lines
			string[] lines = File.ReadAllLines(_summaryFilePath).Where(l => l.StartsWith("KW-A")).ToArray();

			results.Encrypt128Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "KW-AE with AES-128 cipher function") == "PASSED";
			results.Encrypt192Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "KW-AE with AES-192 cipher function") == "PASSED";
			results.Encrypt256Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "KW-AE with AES-256 cipher function") == "PASSED";
			results.Decrypt128Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "KW-AD with AES-128 cipher function") == "PASSED";
			results.Decrypt192Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "KW-AD with AES-192 cipher function") == "PASSED";
			results.Decrypt256Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "KW-AD with AES-256 cipher function") == "PASSED";
			results.Encrypt128InversePassed = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "KW-AE with AES-128 inverse cipher function") == "PASSED";
			results.Encrypt192InversePassed = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "KW-AE with AES-192 inverse cipher function") == "PASSED";
			results.Encrypt256InversePassed = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "KW-AE with AES-256 inverse cipher function") == "PASSED";
			results.Decrypt128InversePassed = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "KW-AD with AES-128 inverse cipher function") == "PASSED";
			results.Decrypt192InversePassed = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "KW-AD with AES-192 inverse cipher function") == "PASSED";
			results.Decrypt256InversePassed = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "KW-AD with AES-256 inverse cipher function") == "PASSED";

			return results;
		}
	}
}