using System.IO;
using System.Linq;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults.Component;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.Component
{
	public class IKEv2_ResultsExtractor : IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "kdf135_Summary.txt";
		private string _summaryFilePath;

		public IKEv2_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public IKEv2_Results Extract()
		{
			IKEv2_Results results = new IKEv2_Results();

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

				//Skip everything that isn't the IKEv2 line
				if (!line.StartsWith("IKE v2 KDF")) continue;

				//Found the IKEv2 line, so the next line will be the result
				line = lines[i + 1];
				results.Passed = line == "\t\t\tPASSED";
				break;
			}

			//Handle error case of IKEv2 line not being found
			if (i == lines.Length) results.InvalidReasons.Add("IKEv2 result not found");

			return results;
		}
	}
}
