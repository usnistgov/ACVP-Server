using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults.AES;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.AES
{
	public class AES_CCM_ResultsExtractor : IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "CCM_Summary.txt";
		private string _summaryFilePath;

		public AES_CCM_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public AES_CCM_Results Extract()
		{
			AES_CCM_Results results = new AES_CCM_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Read the summary file content
			string[] lines = File.ReadAllLines(_summaryFilePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

			string line;

			for (int i = 0; i < lines.Length; i++)
			{
				line = lines[i];

				//Skip everything that something we care about
				if (!line.StartsWith("[")) continue;

				//Pattern is that one line has the flavor, the next line has the (very indented) result

				if (line.StartsWith("[AES-128]")){
					results.Passed128 = lines[++i].Trim() == "Passed";		//Increment i to move to the next line and get the result
					continue;
				}

				if (line.StartsWith("[AES-192]"))
				{
					results.Passed192 = lines[++i].Trim() == "Passed";
					continue;
				}

				if (line.StartsWith("[AES-256]"))
				{
					results.Passed256 = lines[++i].Trim() == "Passed";
					continue;
				}
			}

			return results;
		}
	}
}