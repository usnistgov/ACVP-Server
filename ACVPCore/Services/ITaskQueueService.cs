﻿using ACVPCore.Results;

namespace ACVPCore.Services
{
	public interface ITaskQueueService
	{
		Result AddGenerationTask(GenerationTask task);
		Result AddValidationTask(ValidationTask task);
	}
}