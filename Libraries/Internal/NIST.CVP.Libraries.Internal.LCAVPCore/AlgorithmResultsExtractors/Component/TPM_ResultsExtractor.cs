using System.IO;
using System.Linq;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults.Component;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.Component
{
	public class TPM_ResultsExtractor : IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "kdf135_Summary.txt";
		private string _summaryFilePath;

		public TPM_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public TPM_Results Extract()
		{
			TPM_Results results = new TPM_Results();

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

				//Skip everything that isn't the TPM line
				if (!line.StartsWith("TPM KDF")) continue;

				//Found the TPM line, so the next line will be the result
				line = lines[i + 1];
				results.Passed = line == "\t\t\tPASSED";
				break;
			}

			//Handle error case of TPM line not being found
			if (i == lines.Length) results.InvalidReasons.Add("TPM result not found");

			return results;
		}
	}
}
