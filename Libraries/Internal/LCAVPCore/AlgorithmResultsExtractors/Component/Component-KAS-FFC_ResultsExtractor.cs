using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LCAVPCore.AlgorithmResults;
using LCAVPCore.AlgorithmResults.Component;

namespace LCAVPCore.AlgorithmResultsExtractors.Component
{
	public class Component_KAS_FFC_ResultsExtractor : IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "800-56a_FFC_SummaryFile.txt";
		private string _summaryFilePath;

		public Component_KAS_FFC_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public Component_KAS_FFC_Results Extract()
		{
			Component_KAS_FFC_Results results = new Component_KAS_FFC_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Read the summary file content
			string[] lines = File.ReadAllLines(_summaryFilePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

			List<PassFailResult> sectionResults;

			int i = 0;
			while (i < lines.Length - 1)    //Since we're going to look for the header and the line after, and the last line of the file being a header would be useless, just stop one before the length (which means it is always safe to look at the next line without overflow)
			{
				//Look for a line starting with a * (a header line) followed by a line starting with [FFC (an FFC test result)
				if (lines[i].StartsWith("*") && lines[i + 1].StartsWith("[FFC"))
				{
					//Get the results under this header
					sectionResults = GetTestResults(lines, i);

					//Now need to figure out what those result lines represent. Depends on the header and the name of the first test. If neither functionality or validity header, we don't care about it.
					if (sectionResults.Count > 0)
					{
						if (lines[i].Contains("Functionality"))
						{
							//Test names are all [FFC_PartWeCareAbout_OtherStuff], so split on the _ and take the 2nd member of that array to identify what scheme we have data for
							switch (sectionResults[0].TestName.Split("_".ToCharArray())[1])
							{
								case "EPHEM":
									results.DHEphem_Functionality = sectionResults;
									break;
								case "HYBRID1":
									results.DHHybrid1_Functionality = sectionResults;
									break;
								case "HYBRID1FLOW":
									results.DHHybrid1Flow_Functionality = sectionResults;
									break;
								case "MQV1":
									results.MQV1_Functionality = sectionResults;
									break;
								case "MQV2":
									results.MQV2_Functionality = sectionResults;
									break;
								case "ONEFLOW":
									results.DHOneFlow_Functionality = sectionResults;
									break;
								case "STATIC":
									results.DHStatic_Functionality = sectionResults;
									break;
								default:
									break;	//Should never happen
							}
						}
						else if (lines[i].Contains("Validity"))
						{
							switch (sectionResults[0].TestName.Split("_".ToCharArray())[1])
							{
								case "EPHEM":
									results.DHEphem_Validity = sectionResults;
									break;
								case "HYBRID1":
									results.DHHybrid1_Validity = sectionResults;
									break;
								case "HYBRID1FLOW":
									results.DHHybrid1Flow_Validity = sectionResults;
									break;
								case "MQV1":
									results.MQV1_Validity = sectionResults;
									break;
								case "MQV2":
									results.MQV2_Validity = sectionResults;
									break;
								case "ONEFLOW":
									results.DHOneFlow_Validity = sectionResults;
									break;
								case "STATIC":
									results.DHStatic_Validity = sectionResults;
									break;
								default:
									break;	//Should never happen
							}
						}
					}

					//Increment the loop by the number of results lines parsed. Since i was the index of the header line, this will leave i as the index of the last result line before the next header, meaning the incrementer outside this if is correct regardless of the number of results found
					i += sectionResults.Count;
				}

				i++;	//Move to the next line
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