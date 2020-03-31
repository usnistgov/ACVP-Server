using System;
using Microsoft.AspNetCore.Mvc;
using NIST.CVP.Enumerables;
using NIST.CVP.ExtensionMethods;
using NIST.CVP.MessageQueue;
using NIST.CVP.MessageQueue.Services;
using NIST.CVP.Results;

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