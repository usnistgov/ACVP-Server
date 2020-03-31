using Microsoft.AspNetCore.Mvc;
using NIST.CVP.Enumerables;
using NIST.CVP.ExtensionMethods;
using NIST.CVP.Results;
using NIST.CVP.TaskQueue;
using NIST.CVP.TaskQueue.Services;

namespace Web.Admin.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TaskQueueController : ControllerBase
	{
		private readonly ITaskQueueService _taskQueueService;

		public TaskQueueController(ITaskQueueService taskQueueService)
		{
			_taskQueueService = taskQueueService;
		}

		[HttpGet]
		public ActionResult<WrappedEnumerable<TaskQueueItem>> GetTaskQueue() => _taskQueueService.List().ToWrappedEnumerable();

		[HttpDelete("{taskID}")]
		public Result DeleteTask(long taskID) => _taskQueueService.Delete(taskID);

		//Should be a RestartAll endpoint here too, wasn't sure what to make it
	}
}