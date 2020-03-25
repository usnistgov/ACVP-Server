using System;
using System.IO;
using System.Linq;
using LCAVPCore.AlgorithmResults;
using LCAVPCore.AlgorithmResults.RSA;

namespace LCAVPCore.AlgorithmResultsExtractors.RSA
{
	public class RSAKeyGen186_2_ResultsExtractor : RSA_ResultsExtractorBase, IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "RSA2_Summary.txt";
		private string _summaryFilePath;

		public RSAKeyGen186_2_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public RSAKeyGen_Results Extract()
		{
			RSAKeyGen_Results results = new RSAKeyGen_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Read the summary file content
			string[] lines = File.ReadAllLines(_summaryFilePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

			//186-2 KeyGen includes tabs in the test names, meaning my standard parsing code can't handle it. So do that one with a unique method
			int sectionIndex = Array.IndexOf(lines, "[FIPS 186-2 RSA Key Generation (for use with CMVP 1SUB 2SUB and 4SUB revalidations)]");

			string line;
			string[] parts;
			bool inSection = true;
			int i = 1;
			while (inSection)
			{
				line = lines[sectionIndex + i];

				//Break when get to the next section
				if (line.StartsWith("Summary"))
				{
					inSection = false;
				}
				else
				{
					//Split on the colon in the line, even though it is really part of the test name
					parts = line.Split(":".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

					//Parse the line, skipping the blank lines
					if (parts.Length == 2)
					{
						results.Results.Add(new PassFailResult(parts[0].Trim(), parts[1].Trim().ToLower() == "passed"));
					}

					i++;
				}
			}

			return results;
		}
	}
}
