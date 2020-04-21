using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults.TDES;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.TDES
{
	public class TDES_CFBP1_ResultsExtractor : IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "TDES_Summary.txt";
		private string _summaryFilePath;

		public TDES_CFBP1_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public TDES_CFBP1_Results Extract()
		{
			TDES_CFBP1_Results results = new TDES_CFBP1_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Read the summary file content
			string[] lines = File.ReadAllLines(_summaryFilePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

			//Break out the CFBP1 sections
			string line;

			for (int i = 0; i < lines.Length; i++)
			{
				line = lines[i];

				//Skip everything that isn't a CFBP1 mode header
				if (!line.StartsWith("[Mode CFBP1")) continue;

				//Move to next line and to get the action (Encrypt/Decrypt)
				line = lines[++i];      //Dangerous in theory, but will never actually overflow

				if (line.StartsWith("[State Encrypt]"))
				{
					//Get the appropriate lines of results
					results.Encrypt = GetTestResults(lines, i, AlgorithmTestNames.TDES_Encrypt);

					if (results.Encrypt == null)
					{
						results.InvalidReasons.Add("Incorrect or missing test result");
						break;
					}

					//Increment the main loop by the magic number of results + 1 for the [State Encrypt] line, and move to that line
					i += 1 + results.Encrypt.Count;
					line = lines[i];
				}

				if (line.StartsWith("[State Decrypt]"))
				{
					//Get the appropriate lines of results
					results.Decrypt = GetTestResults(lines, i, AlgorithmTestNames.TDES_Decrypt);

					if (results.Decrypt == null)
					{
						results.InvalidReasons.Add("Incorrect or missing test result");
						break;
					}

					//Increment the main loop by the magic number of results + 1 for the [State Decrypt] line
					i += 1 + results.Decrypt.Count;
				}
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