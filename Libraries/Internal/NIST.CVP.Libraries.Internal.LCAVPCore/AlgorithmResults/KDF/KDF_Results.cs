namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults.KDF
{
	public class KDF_Results : AlgorithmResultsBase
	{
		public bool Counter_CMAC_AES128Passed { get; set; }
		public bool Counter_CMAC_AES192Passed { get; set; }
		public bool Counter_CMAC_AES256Passed { get; set; }
		public bool Counter_CMAC_TDES2Passed { get; set; }
		public bool Counter_CMAC_TDES3Passed { get; set; }
		public bool Counter_HMAC_SHA1Passed { get; set; }
		public bool Counter_HMAC_SHA224Passed { get; set; }
		public bool Counter_HMAC_SHA256Passed { get; set; }
		public bool Counter_HMAC_SHA384Passed { get; set; }
		public bool Counter_HMAC_SHA512Passed { get; set; }

		public bool Pipeline_CMAC_AES128Passed { get; set; }
		public bool Pipeline_CMAC_AES192Passed { get; set; }
		public bool Pipeline_CMAC_AES256Passed { get; set; }
		public bool Pipeline_CMAC_TDES2Passed { get; set; }
		public bool Pipeline_CMAC_TDES3Passed { get; set; }
		public bool Pipeline_HMAC_SHA1Passed { get; set; }
		public bool Pipeline_HMAC_SHA224Passed { get; set; }
		public bool Pipeline_HMAC_SHA256Passed { get; set; }
		public bool Pipeline_HMAC_SHA384Passed { get; set; }
		public bool Pipeline_HMAC_SHA512Passed { get; set; }

		public bool Feedback_CMAC_AES128Passed { get; set; }
		public bool Feedback_CMAC_AES192Passed { get; set; }
		public bool Feedback_CMAC_AES256Passed { get; set; }
		public bool Feedback_CMAC_TDES2Passed { get; set; }
		public bool Feedback_CMAC_TDES3Passed { get; set; }
		public bool Feedback_HMAC_SHA1Passed { get; set; }
		public bool Feedback_HMAC_SHA224Passed { get; set; }
		public bool Feedback_HMAC_SHA256Passed { get; set; }
		public bool Feedback_HMAC_SHA384Passed { get; set; }
		public bool Feedback_HMAC_SHA512Passed { get; set; }
	}
}