using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults.Component;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.Component
{
	public class ECC_CDH_ResultsExtractor : IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "800-56a_ECC_SummaryFile.txt";
		private string _summaryFilePath;

		public ECC_CDH_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public ECC_CDH_Results Extract()
		{
			ECC_CDH_Results results = new ECC_CDH_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Read the summary file content
			string[] lines = File.ReadAllLines(_summaryFilePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

			//Find the one header in the file that we care about
			int headerIndex = Array.IndexOf(lines, "****************** ECC Cofactor Diffie-Hellman(ECC CDH) Component Testing ******************");

			//Catch if not found
			if (headerIndex == -1)
			{
				results.InvalidReasons.Add("ECC CDH results not found");
			}
			else
			{
				results.Results = GetTestResults(lines, headerIndex);
			}

			return results;
		}

		private List<PassFailResult> GetTestResults(string[] lines, int headerIndex)
		{
			List<PassFailResult> results = new List<PassFailResult>();

			string[] parts;
			int i = headerIndex + 1;    //Start at the line after the *** header line

			while (i < lines.Length && lines[i].StartsWith("["))    //Stop the loop when the line doesn't start with [, which means we've reached the next section
			{
				//Split based on the tabs, should give us the test name and the result value
				parts = lines[i].Split("\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

				//Create a new result object based on the test name and whether or not it passed
				results.Add(new PassFailResult(parts[0], parts[1].Trim() == "PASSED"));

				i++;
			}

			return results;
		}
	}
}