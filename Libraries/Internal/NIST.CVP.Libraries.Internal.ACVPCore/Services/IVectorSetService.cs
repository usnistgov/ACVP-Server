using System;
using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;
using NIST.CVP.Libraries.Shared.Results;


namespace NIST.CVP.Libraries.Internal.ACVPCore.Services
{
	public interface IVectorSetService
	{
		Result Cancel(long vectorSetID);
		Result InsertSubmittedAnswers(long vectorSetID, string results);
		Result Create(long vectorSetID, long testSessionID, string generatorVersion, long algorithmID, string capabilities);
		Result UpdateStatus(long vectorSetID, VectorSetStatus status);
		Result RecordError(long vectorSetID, string errorMessage);
		List<(long ID, long AlgorithmID, VectorSetStatus Status, string ErrorMessage)> GetVectorSetsForTestSession(long testSessionID);
		VectorSet GetVectorSet(long vectorSetId);
		string GetVectorFileJson(long vectorSetId, VectorSetJsonFileTypes fileType);
		List<(VectorSetJsonFileTypes FileType, string Content, DateTime CreatedOn)> GetVectorFileJson(long vectorSetID);
		Result Archive(long vectorSetID);
		List<long> GetVectorSetsToArchive();
		Result RemoveVectorFileJson(long vectorSetId, VectorSetJsonFileTypes fileType);
	}
}