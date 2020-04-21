using System;
using System.Collections.Generic;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.ECDSA
{
	public class ECDSA_ResultsExtractorBase : IAlgorithmResultsExtractor
	{
		public List<PassFailResult> ExtractLinesInSection(string[] lines, int headerIndex)
		{
			List<PassFailResult> results = new List<PassFailResult>();

			if (headerIndex != -1)      //Check that the header was actually found (-1 is returned by Array.IndexOf if not found)
			{
				int i = headerIndex + 1;
				string[] parts;

				while (i < lines.Length && !lines[i].StartsWith("Summary"))
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