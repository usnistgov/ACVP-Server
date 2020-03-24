using System.IO;
using System.Linq;
using LCAVPCore.AlgorithmResults.SHA_3;

namespace LCAVPCore.AlgorithmResultsExtractors.SHA_3
{
	public class SHA3_384_ResultsExtractor : IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "SHA3_Summary.txt";
		private string _summaryFilePath;

		public SHA3_384_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public SHA3_384_Results Extract()
		{
			SHA3_384_Results results = new SHA3_384_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Read the summary file content
			string[] lines = File.ReadAllLines(_summaryFilePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

			//Just grab the 3 lines I care about
			string passLineValue = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "[SHA3_384]");
			string byteLineValue = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "[Byte_oriented only]");

			//Special note line is harder to get because identical text appears twice in the SHA-3 summary file - once for SHA-3, once for SHAKE
			string nullLineValue = null;
			bool inSHAKE = false;
			foreach (string line in lines)
			{
				//Read the value, but only if know we're not in the SHAKE section
				if (!inSHAKE && line.StartsWith("[Special Note (will be displayed on website)]"))
				{
					nullLineValue = ParsingHelper.GetValueStringFromSummaryLine(line);
					break;
				}

				//Flag when we're in the SHAKE section
				if (line.StartsWith("SHA_3 Extendable_Output Functions (XOFs) information:")) inSHAKE = true;
			}

			//Catch any errors if they are not present - the null line is only written if it does not support null message, so no error on that one
			if (passLineValue == null) results.InvalidReasons.Add("Missing algorithm pass/fail");
			if (string.IsNullOrWhiteSpace(byteLineValue)) results.InvalidReasons.Add("Missing byte oriented line");

			if (!results.Valid) return results;

			//Set the results
			results.Pass = (passLineValue == "Passed");
			results.ByteOrientedOnly = (byteLineValue == "TRUE");
			results.DoesNotSupportNullMessage = (nullLineValue == "Does not support null message");

			return results;
		}
	}
}
