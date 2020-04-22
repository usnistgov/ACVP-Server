using System;
using System.Collections.Generic;
using NIST.CVP.Libraries.Internal.ACVPCore.Providers;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;
using NIST.CVP.Libraries.Shared.Results;


namespace NIST.CVP.Libraries.Internal.ACVPCore.Services
{
	public class VectorSetService : IVectorSetService
	{
		private readonly IVectorSetProvider _vectorSetProvider;
		private readonly ITestSessionService _testSessionService;

		public VectorSetService(IVectorSetProvider vectorSetProvider, ITestSessionService testSessionService)
		{
			_vectorSetProvider = vectorSetProvider;
			_testSessionService = testSessionService;
		}

		public Result Cancel(long vectorSetID)
		{
			//Cancel the vector set
			Result result = _vectorSetProvider.Cancel(vectorSetID);

			if (result.IsSuccess)
			{
				//Potentially update the Test Session status
				result = _testSessionService.UpdateStatusFromVectorSetsWithVectorSetID(vectorSetID);
			}

			return result;
		}

		public Result Create(long vectorSetID, long testSessionID, string generatorVersion, long algorithmID, string capabilities)
		{
			Result result;

			//Insert the vector set record
			result = _vectorSetProvider.Insert(vectorSetID, testSessionID, generatorVersion, algorithmID);

			if (result.IsSuccess)
			{
				//Insert the capabilities json
				result = _vectorSetProvider.InsertVectorSetJson(vectorSetID, VectorSetJsonFileTypes.Capabilities, capabilities);
			}

			return result;
		}

		public Result InsertSubmittedAnswers(long vectorSetID, string results)
		{
			return _vectorSetProvider.InsertVectorSetJson(vectorSetID, VectorSetJsonFileTypes.SubmittedAnswers, results);
		}

		public Result UpdateStatus(long vectorSetID, VectorSetStatus status) => _vectorSetProvider.UpdateStatus(vectorSetID, status, null);

		public Result RecordError(long vectorSetID, string errorMessage)
		{
			return _vectorSetProvider.UpdateStatus(vectorSetID, VectorSetStatus.Error, errorMessage);
		}

		public List<(long ID, long AlgorithmID, VectorSetStatus Status, string ErrorMessage)> GetVectorSetsForTestSession(long testSessionID) => _vectorSetProvider.GetVectorSetIDsForTestSession(testSessionID);

		public VectorSet GetVectorSet(long vectorSetId)
		{
			var result = _vectorSetProvider.GetVectorSet(vectorSetId);
			
			if (result == null)
				return null;
			
			result.JsonFilesAvailable = _vectorSetProvider.GetVectorSetJsonFilesAvailable(vectorSetId);

			// We can potentially allow the vector set to be resubmitted to the task queue, depending on the nature of the failure (transient or not).
			if (result.Status == VectorSetStatus.Error || result.Status == VectorSetStatus.Failed)
			{
				// Allow for the resubmitting to the task queue a generation task 
				if (result.JsonFilesAvailable.Contains(VectorSetJsonFileTypes.Capabilities) &&
				    !result.JsonFilesAvailable.Contains(VectorSetJsonFileTypes.Prompt))
				{
					result.ResetOption = VectorSetResetOption.ResetToGenerate;
				}
				
				// Allow for the resubmitting to the task queue a validation task
				if (result.JsonFilesAvailable.Contains(VectorSetJsonFileTypes.SubmittedAnswers) &&
				    !result.JsonFilesAvailable.Contains(VectorSetJsonFileTypes.Validation))
				{
					result.ResetOption = VectorSetResetOption.ResetToValidate;
				}
			}
			
			return result;
		}

		public string GetVectorFileJson(long vectorSetId, VectorSetJsonFileTypes fileType)
		{
			return _vectorSetProvider.GetVectorFileJson(vectorSetId, fileType);
		}

		public List<(VectorSetJsonFileTypes FileType, string Content, DateTime CreatedOn)> GetVectorFileJson(long vectorSetID) => _vectorSetProvider.GetVectorFileJson(vectorSetID);

		public Result Archive(long vectorSetID) => _vectorSetProvider.Archive(vectorSetID);

		public List<long> GetVectorSetsToArchive() => _vectorSetProvider.GetVectorSetsToArchive();

		public Result RemoveVectorFileJson(long vectorSetId, VectorSetJsonFileTypes fileType) => _vectorSetProvider.RemoveVectorFileJson(vectorSetId, fileType);
	}
}
