using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LCAVPCore.AlgorithmResults;
using LCAVPCore.AlgorithmResults.AES;

namespace LCAVPCore.AlgorithmResultsExtractors.AES
{
	public class AES_CFB8_ResultsExtractor : IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "AES_Summary.txt";
		private string _summaryFilePath;

		public AES_CFB8_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public AES_CFB8_Results Extract()
		{
			AES_CFB8_Results results = new AES_CFB8_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Read the summary file content
			string[] lines = File.ReadAllLines(_summaryFilePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

			//Break out the CFB8 sections
			string line;
			int keySize;
			List<PassFailResult> currentKeySizeActionResults;

			for (int i = 0; i < lines.Length; i++)
			{
				line = lines[i];

				//Skip everything that isn't a CFB8 mode header
				if (!line.StartsWith("[Mode CFB8")) continue;

				//Have a mode header line. Make sure it is good and get the key size
				if (line.Length < 17 || !int.TryParse(line.Substring(13, 3), out keySize))
				{
					results.InvalidReasons.Add($"Invalid mode line: {line}");
					break;
				}

				//Move to next line and to get the action (Encrypt/Decrypt)
				line = lines[++i];      //Dangerous in theory, but will never actually overflow

				if (line.StartsWith("[State Encrypt]"))
				{
					//Get the right object within the results
					//currentKeySizeActionResults = GetKeySizeActionResultObject(keySize, "Encrypt", results);

					//Read the 6 lines of results
					currentKeySizeActionResults = GetTestResults(lines, i);

					if (currentKeySizeActionResults == null)
					{
						results.InvalidReasons.Add("Incorrect or missing test result");
						break;
					}

					//Assign it to the proper thing
					switch (keySize)
					{
						case 128:
							results.Encrypt128 = currentKeySizeActionResults;
							break;

						case 192:
							results.Encrypt192 = currentKeySizeActionResults;
							break;

						case 256:
							results.Encrypt256 = currentKeySizeActionResults;
							break;

						default:
							results.InvalidReasons.Add("Invalid key size");
							return results;
					}

					//Increment the main loop by the magic number of results + 1 for the [State Encrypt] line, and move to that line
					i += 7;
					line = lines[i];
				}

				if (line.StartsWith("[State Decrypt]"))
				{
					//Get the right object within the results
					//currentKeySizeActionResults = GetKeySizeActionResultObject(keySize, "Decrypt", results);

					//Read the 6 lines of results
					currentKeySizeActionResults = GetTestResults(lines, i);

					if (currentKeySizeActionResults == null)
					{
						results.InvalidReasons.Add("Incorrect or missing test result");
						break;
					}

					switch (keySize)
					{
						case 128:
							results.Decrypt128 = currentKeySizeActionResults;
							break;

						case 192:
							results.Decrypt192 = currentKeySizeActionResults;
							break;

						case 256:
							results.Decrypt256 = currentKeySizeActionResults;
							break;

						default:
							results.InvalidReasons.Add("Invalid key size");
							return results;
					}

					//Increment the main loop by the magic number of results + 1 for the [State Decrypt] line
					i += 7;
				}
			}

			return results;
		}

		private List<PassFailResult> GetTestResults(string[] lines, int stateLineIndex)
		{
			//Get the next 6 lines
			string[] relevantLines = lines.Skip(stateLineIndex + 1).Take(6).ToArray();      //The +1 is to make it start with the line after the [State line

			if (relevantLines.Length != 6) return null;

			List<PassFailResult> results = new List<PassFailResult>(6);

			string[] testNames = AlgorithmTestNames.AES_CFB8;

			string[] parts;

			//Loop through all the test names trying to find the result
			for (int i = 0; i < 6; i++)
			{
				//Split based on the tabs, should give us the test name and the result value
				parts = relevantLines[i].Split("\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

				//Check that test name is the right one for this line
				if (parts[0].Trim() != testNames[i]) return null;

				//Create a new result object based on the test name and whether or not it passed
				results.Add(new PassFailResult(testNames[i], parts[1].Trim().ToLower() == "passed"));
			}

			return results;
		}

		private List<PassFailResult> GetKeySizeActionResultObject(int keySize, string action, AES_CFB8_Results results)
		{
			List<PassFailResult> x = null;
			switch (action)
			{
				case "Encrypt":
					switch (keySize)
					{
						case 128: x = results.Encrypt128; break;
						case 192: x = results.Encrypt192; break;
						case 256: x = results.Encrypt256; break;
					}
					break;

				case "Decrypt":
					switch (keySize)
					{
						case 128: x = results.Decrypt128; break;
						case 192: x = results.Decrypt192; break;
						case 256: x = results.Decrypt256; break;
					}
					break;
			}
			return x;
		}
	}
}