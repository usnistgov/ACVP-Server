using System.IO;
using System.Linq;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults.SHA_3;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.SHA_3
{
	public class SHAKE_128_ResultsExtractor : IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "SHA3_Summary.txt";
		private string _summaryFilePath;

		public SHAKE_128_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public SHAKE_128_Results Extract()
		{
			SHAKE_128_Results results = new SHAKE_128_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Read the summary file content
			string[] lines = File.ReadAllLines(_summaryFilePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

			//Just grab the 3 lines I care about
			string passLineValue = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "[SHAKE128]");
			string inputByteLineValue = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "[Input Message: Byte_oriented only]");
			string outputByteLineValue = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "[Output Message: Byte_oriented only]");

			//Special note line is harder to get because identical text appears twice in the SHA-3 summary file - once for SHA-3, once for SHAKE
			string nullLineValue = null;
			bool inSHAKE = false;
			foreach (string line in lines)
			{
				//Read the value, but only if know we're in the SHAKE section
				if (inSHAKE && line.StartsWith("[Special Note (will be displayed on website)]"))
				{
					nullLineValue = ParsingHelper.GetValueStringFromSummaryLine(line);
					break;
				}

				//Flag when we're in the SHAKE section
				if (line.StartsWith("SHA_3 Extendable_Output Functions (XOFs) information:")) inSHAKE = true;
			}

			//Catch any errors if they are not present - the null line is only written if it does not support null message, so no error on that one
			if (passLineValue == null) results.InvalidReasons.Add("Missing algorithm pass/fail\n");
			if (string.IsNullOrWhiteSpace(inputByteLineValue)) results.InvalidReasons.Add("Missing input byte oriented line\n");
			if (string.IsNullOrWhiteSpace(outputByteLineValue)) results.InvalidReasons.Add("Missing output byte oriented line\n");

			if (!results.Valid) return results;

			//Set the results
			results.Pass = (passLineValue == "Passed");
			results.InputByteOrientedOnly = (inputByteLineValue == "TRUE");
			results.OutputByteOrientedOnly = (outputByteLineValue == "TRUE");
			results.DoesNotSupportNullMessage = (nullLineValue == "Does not support null message");

			return results;
		}
	}
}
