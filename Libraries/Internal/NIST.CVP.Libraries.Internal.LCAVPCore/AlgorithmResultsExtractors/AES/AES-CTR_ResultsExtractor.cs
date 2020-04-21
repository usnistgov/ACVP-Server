using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults.AES;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.AES
{
	public class AES_CTR_ResultsExtractor : IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "AES_Summary.txt";
		private string _summaryFilePath;

		public AES_CTR_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public AES_CTR_Results Extract()
		{
			AES_CTR_Results results = new AES_CTR_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Read the summary file content
			string[] lines = File.ReadAllLines(_summaryFilePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

			//Break out the CTR sections
			string line;
			int keySize;
			string[] parts;

			for (int i = 0; i < lines.Length; i++)
			{
				line = lines[i];

				//Skip everything that isn't a CTR mode header
				if (!line.StartsWith("[Mode CTR")) continue;

				//Have a mode header line. Make sure it is good and get the key size
				if (line.Length < 16 || !int.TryParse(line.Substring(12, 3), out keySize))
				{
					results.InvalidReasons.Add($"Invalid mode line: {line}");
					break;
				}


				//Move to next line and to get the result for this key size
				line = lines[++i];      //Dangerous in theory, but will never actually overflow unless the file is horribly broken

				parts = line?.Split("\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
				if (parts?.Length == 2 && parts[0].Trim() == "Corresponding forward cipher function Tests")
				{
					switch (keySize)
					{
						case 128:
							results.Passed128 = parts[1] == "PASSED";
							break;
						case 192:
							results.Passed192 = parts[1] == "PASSED";
							break;
						case 256:
							results.Passed256 = parts[1] == "PASSED";
							break;
						default:	//Not that this should ever happen, but if it does, ignore it
							break;
					}

				}
				else results.InvalidReasons.Add($"Corresponding forward cipher function Tests line not found for key size {keySize}\n");

				//Loop will continue and find the other modes
			}

			return results;
		}
	}
}