using System;
using Microsoft.AspNetCore.Mvc;
using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Shared.ExtensionMethods;
using NIST.CVP.Libraries.Internal.MessageQueue;
using NIST.CVP.Libraries.Internal.MessageQueue.Services;
using NIST.CVP.Libraries.Shared.Results;

namespace Web.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageQueueController : ControllerBase
    {
        private readonly IMessageQueueService _messageQueueService;

        public MessageQueueController(IMessageQueueService messageQueueService)
        {
            _messageQueueService = messageQueueService;
        }

        [HttpGet]
        public ActionResult<WrappedEnumerable<MessageQueueItem>> GetMessageQueue() => _messageQueueService.List().ToWrappedEnumerable();

        [HttpGet("{messageID}")]
        public string GetMessagePayload(Guid messageID) => _messageQueueService.GetMessagePayload(messageID);

        [HttpDelete("{messageID}")]
        public Result DeleteMessage(Guid messageID)
        {
            _messageQueueService.DeleteMessage(messageID);
            return new Result();
        }
    }
}