using System.IO;
using System.Linq;
using LCAVPCore.AlgorithmResults.Component;

namespace LCAVPCore.AlgorithmResultsExtractors.Component
{
	public class RSASP1_ResultsExtractor : IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "RSA2_Summary.txt";
		private string _summaryFilePath;

		public RSASP1_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public RSASP1_Results Extract()
		{
			RSASP1_Results results = new RSASP1_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Read the summary file content
			string[] lines = File.ReadAllLines(_summaryFilePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

			int i = 0;
			//Skip everything until the header line
			while (i < lines.Length && !lines[i].StartsWith("[FIPS 186-4 RSA Signature Primitive - RSASP1 Component]")){
				i++;
			}

			//Handle it not being found
			if (i == lines.Length)
			{
				results.InvalidReasons.Add("RSASP1 result not found");
				return results;
			}

			//Next line is the one we care about - hopefully
			results.Passed = ParsingHelper.GetValueStringFromSummaryLine(lines[i + 1]) == "Passed";

			return results;
		}
	}
}
