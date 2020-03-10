using System.IO;
using System.Linq;
using LCAVPCore.AlgorithmResults.Component;

namespace LCAVPCore.AlgorithmResultsExtractors.Component
{
	public class RSADP_ResultsExtractor : IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "RSADP_Summary.txt";
		private string _summaryFilePath;

		public RSADP_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public RSADP_Results Extract()
		{
			RSADP_Results results = new RSADP_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Since there's only 1 line in the file that matters, and no complications from other modes/algorithms, just find that one line and see if it passed
			results.Passed = File.ReadAllLines(_summaryFilePath).Where(l => !string.IsNullOrWhiteSpace(l)).Any(l => l.TrimStart().StartsWith("Mod 2048") && l.TrimEnd().EndsWith("Passed"));

			return results;
		}
	}
}
