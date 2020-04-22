using System.IO;
using System.Linq;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults.SHS;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.SHS
{
	public class SHA_384_ResultsExtractor : IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "SHA_Summary.txt";
		private string _summaryFilePath;

		public SHA_384_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public SHA_384_Results Extract()
		{
			SHA_384_Results results = new SHA_384_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Read the summary file content
			string[] lines = File.ReadAllLines(_summaryFilePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

			//Just grab the 3 lines I care about
			string passLineValue = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "[SHA-384]");
			string byteLineValue = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "[Byte-oriented only]");
			string nullLineValue = ParsingHelper.GetValueStringFromSummaryLinesByKey(lines, "[Special Note (will be displayed on website)]");

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