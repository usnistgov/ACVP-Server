using System.IO;
using System.Linq;
using LCAVPCore.AlgorithmResults.Component;

namespace LCAVPCore.AlgorithmResultsExtractors.Component
{
	public class TLS_ResultsExtractor : IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "kdf135_Summary.txt";
		private string _summaryFilePath;

		public TLS_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public TLS_Results Extract()
		{
			TLS_Results results = new TLS_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Read the summary file content
			string[] lines = File.ReadAllLines(_summaryFilePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

			//Break out the CBC sections
			string line;
			int i;

			for (i = 0; i < lines.Length; i++)
			{
				line = lines[i];

				//Skip everything that isn't the TLS line
				if (!line.StartsWith("TLS KDF")) continue;

				//Found the TLS line, so the next line will be the result
				line = lines[i + 1];
				results.Passed = line == "\t\t\tPASSED";
				break;
			}

			//Handle error case of TLS line not being found
			if (i == lines.Length) results.InvalidReasons.Add("TLS result not found");

			return results;
		}
	}
}
