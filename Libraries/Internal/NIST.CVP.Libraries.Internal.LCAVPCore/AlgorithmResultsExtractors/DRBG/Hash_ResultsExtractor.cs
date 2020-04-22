using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults.DRBG;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.DRBG
{
	public class Hash_ResultsExtractor : IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "DRBG800-90A_Summary.txt";
		private string _summaryFilePath;

		public Hash_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public Hash_Results Extract()
		{
			Hash_Results results = new Hash_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Read the summary file content
			string[] lines = File.ReadAllLines(_summaryFilePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

			//Break out the CBC sections
			for (int i = 0; i < lines.Length; i++)
			{
				//Skip everything that isn't the Hash mode header
				if (!lines[i].StartsWith("[Hash_DRBG")) continue;

				//Get the appropriate lines of results
				results.Results = GetTestResults(lines, i, AlgorithmTestNames.DRBG_Hash);

				break;
			}

			return results;
		}

		private List<PassFailResult> GetTestResults(string[] lines, int stateLineIndex, string[] testNames)
		{
			//Get the number of lines we want to grab, based on the testNames passed in
			int numTestLines = testNames.Length;

			string[] relevantLines = lines.Skip(stateLineIndex + 1).Take(numTestLines).ToArray();      //The +1 is to make it start with the line after the [State line

			if (relevantLines.Length != numTestLines) return null;

			List<PassFailResult> results = new List<PassFailResult>(numTestLines);

			string[] parts;

			//Loop through all the test names trying to find the result
			for (int i = 0; i < numTestLines; i++)
			{
				//Split based on the tabs, should give us the test name and the result value
				parts = relevantLines[i].Split("\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

				//Check that test name is the right one for this line
				if (parts[0].Trim() != testNames[i]) return null;

				if (parts.Length == 1)  //No result is present, so treat it like a fail
				{
					results.Add(new PassFailResult(testNames[i], false));
				}
				else
				{
					//Create a new result object based on the test name and whether or not it passed
					results.Add(new PassFailResult(testNames[i], parts[1].Trim().ToLower() == "passed"));
				}
			}

			return results;
		}
	}
}
