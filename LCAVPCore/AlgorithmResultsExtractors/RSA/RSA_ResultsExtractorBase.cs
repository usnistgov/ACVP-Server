using System;
using System.Collections.Generic;
using System.Linq;
using LCAVPCore.AlgorithmResults;

namespace LCAVPCore.AlgorithmResultsExtractors.RSA
{
	public class RSA_ResultsExtractorBase
	{
		public List<PassFailResult> GetTestResults(string[] lines, int stateLineIndex, string[] testNames)
		{
			//Get the number of lines we want to grab, based on the testNames passed in
			int numTestLines = testNames.Length;

			string[] relevantLines = lines.Skip(stateLineIndex + 1).Take(numTestLines).ToArray();      //The +1 is to make it start with the line after the mode line

			if (relevantLines.Length != numTestLines) return null;

			List<PassFailResult> results = new List<PassFailResult>(numTestLines);

			string[] parts;

			//Loop through all the test names trying to find the result
			for (int i = 0; i < numTestLines; i++)
			{
				//Split based on the tabs, should give us the test name and the result value
				parts = relevantLines[i].Split("\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

				//Create a new result object based on the test name and whether or not it passed, including checking that the test name is the expected one. This is a bit of a kludge to handle the extra colon injected into the 186-4 KeyGen BothPC test names under certain conditions when the summary file is created
				results.Add(new PassFailResult(testNames[i], parts[0].Trim() == testNames[i] && parts[1].Trim().ToLower() == "passed"));
			}

			return results;
		}
	}
}
