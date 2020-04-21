using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults.Component;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.Component
{
	public class ECDSASigGenComponent_ResultsExtractor : IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "ECDSA2_Summary.txt";
		private string _summaryFilePath;

		public ECDSASigGenComponent_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public ECDSASigGen_Results Extract()
		{
			ECDSASigGen_Results results = new ECDSASigGen_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Read the summary file content
			string[] lines = File.ReadAllLines(_summaryFilePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

			//Find the header
			int headerIndex = Array.IndexOf(lines, "[Signature Generation Component Only]");

			//Since there is a variable number of tests in the summary file it is a little tricky to get the lines we care about - it may be fewer than the number possible
			//Just read all the lines until the end of the section (the summary tag) and add to the results

			results.Results = new List<PassFailResult>();

			if (headerIndex != -1)      //Check that the header was actually found (-1 is returned by Array.IndexOf if not found)
			{
				int i = headerIndex + 1;
				string[] parts;

				while (i < lines.Length && !lines[i].StartsWith("Summary"))
				{
					//Split based on the tabs, should give us the test name and the result value
					parts = lines[i].Split("\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

					//Create a new result object based on the test name and whether or not it passed
					results.Results.Add(new PassFailResult(parts[0].Trim(), parts[1].Trim() == "Passed"));

					i++;
				}
			}

			return results;
		}
	}
}