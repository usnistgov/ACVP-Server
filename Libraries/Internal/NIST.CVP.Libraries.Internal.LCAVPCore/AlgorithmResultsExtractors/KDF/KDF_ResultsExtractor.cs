using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults.KDF;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.KDF
{
	public class KDF_ResultsExtractor : IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "SP800_108KDF_Summary.txt";
		private string _summaryFilePath;

		public KDF_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public KDF_Results Extract()
		{
			KDF_Results results = new KDF_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Read the summary file content
			string[] lines = File.ReadAllLines(_summaryFilePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

			//Due to the way the file is created, unusual processing is needed. There are 3 sections, but they will only have child lines if they actually tested that section. The child lines are identical between the 3 sections, so must constrain searching to each section

			//Get the indices of all the critical lines
			int counterHeaderIndex = Array.IndexOf(lines, "[KDF in Counter Mode]");
			int doublePipelineHeaderIndex = Array.IndexOf(lines, "[KDF in Double Pipeline Mode]");
			int feedbackHeaderIndex = Array.IndexOf(lines, "[KDF in Feedback Mode]");
			int endOfFileIndex = lines.Length;

			//Make sure all the headers were found
			if (counterHeaderIndex == -1)
			{
				results.InvalidReasons.Add("[KDF in Counter Mode] header not found");
			}
			if (doublePipelineHeaderIndex == -1)
			{
				results.InvalidReasons.Add("[KDF in Double Pipeline Mode] header not found");
			}
			if (feedbackHeaderIndex == -1)
			{
				results.InvalidReasons.Add("[KDF in Feedback Mode] header not found");
			}

			if (results.InvalidReasons.Count != 0) return results;


			//Counter
			var relevantLines = new ArraySegment<string>(lines, counterHeaderIndex, feedbackHeaderIndex - counterHeaderIndex);
			results.Counter_CMAC_AES128Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "PRF=CMAC_AES128") == "Passed";
			results.Counter_CMAC_AES192Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "PRF=CMAC_AES192") == "Passed";
			results.Counter_CMAC_AES256Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "PRF=CMAC_AES256") == "Passed";
			results.Counter_CMAC_TDES2Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "PRF=CMAC_TDES2") == "Passed";
			results.Counter_CMAC_TDES3Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "PRF=CMAC_TDES3") == "Passed";
			results.Counter_HMAC_SHA1Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "PRF=HMAC_SHA1") == "Passed";
			results.Counter_HMAC_SHA224Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "PRF=HMAC_SHA224") == "Passed";
			results.Counter_HMAC_SHA256Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "PRF=HMAC_SHA256") == "Passed";
			results.Counter_HMAC_SHA384Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "PRF=HMAC_SHA384") == "Passed";
			results.Counter_HMAC_SHA512Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "PRF=HMAC_SHA512") == "Passed";

			//Feedback
			relevantLines = new ArraySegment<string>(lines, feedbackHeaderIndex, doublePipelineHeaderIndex - feedbackHeaderIndex);
			results.Feedback_CMAC_AES128Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "PRF=CMAC_AES128") == "Passed";
			results.Feedback_CMAC_AES192Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "PRF=CMAC_AES192") == "Passed";
			results.Feedback_CMAC_AES256Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "PRF=CMAC_AES256") == "Passed";
			results.Feedback_CMAC_TDES2Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "PRF=CMAC_TDES2") == "Passed";
			results.Feedback_CMAC_TDES3Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "PRF=CMAC_TDES3") == "Passed";
			results.Feedback_HMAC_SHA1Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "PRF=HMAC_SHA1") == "Passed";
			results.Feedback_HMAC_SHA224Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "PRF=HMAC_SHA224") == "Passed";
			results.Feedback_HMAC_SHA256Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "PRF=HMAC_SHA256") == "Passed";
			results.Feedback_HMAC_SHA384Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "PRF=HMAC_SHA384") == "Passed";
			results.Feedback_HMAC_SHA512Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "PRF=HMAC_SHA512") == "Passed";

			//Pipeline
			relevantLines = new ArraySegment<string>(lines, doublePipelineHeaderIndex, endOfFileIndex - doublePipelineHeaderIndex);
			results.Pipeline_CMAC_AES128Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "PRF=CMAC_AES128") == "Passed";
			results.Pipeline_CMAC_AES192Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "PRF=CMAC_AES192") == "Passed";
			results.Pipeline_CMAC_AES256Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "PRF=CMAC_AES256") == "Passed";
			results.Pipeline_CMAC_TDES2Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "PRF=CMAC_TDES2") == "Passed";
			results.Pipeline_CMAC_TDES3Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "PRF=CMAC_TDES3") == "Passed";
			results.Pipeline_HMAC_SHA1Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "PRF=HMAC_SHA1") == "Passed";
			results.Pipeline_HMAC_SHA224Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "PRF=HMAC_SHA224") == "Passed";
			results.Pipeline_HMAC_SHA256Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "PRF=HMAC_SHA256") == "Passed";
			results.Pipeline_HMAC_SHA384Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "PRF=HMAC_SHA384") == "Passed";
			results.Pipeline_HMAC_SHA512Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "PRF=HMAC_SHA512") == "Passed";

			return results;
		}
	}
}