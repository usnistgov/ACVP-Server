using System;
using System.Collections.Generic;
using LCAVPCore.AlgorithmResults;

namespace LCAVPCore.AlgorithmResultsExtractors.DSA
{
	public class DSA_ResultsExtractorBase
	{
		protected List<PassFailResult> ExtractModResultsUnderHeader(string[] lines, int headerIndex)
		{
			//All test result lines start with \tMod, whether under a proper section header or a PQG subsection header. So starting after the header index and stopping when the line does not start with \tMod, convert the lines to PassFailResults
			List<PassFailResult> results = new List<PassFailResult>();

			if (headerIndex != -1) {		//Check that the header was actually found (-1 is returned by Array.IndexOf if not found)
				int i = headerIndex + 1;
				string[] parts;

				while (i < lines.Length && lines[i].StartsWith("\tMod"))
				{
					//Split based on the tabs, should give us the test name and the result value
					parts = lines[i].Split("\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

					//Create a new result object based on the test name and whether or not it passed
					results.Add(new PassFailResult(parts[0].Trim(), parts[1].Trim() == "Passed"));

					i++;
				}
			}

			return results;
		}
	}
}