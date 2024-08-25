using Microsoft.AspNetCore.Mvc;
using WebApi.DAL.Models;
using WebApi.DAL.Repositories.MessageRepository;
using WebApi.Services.SocketConnectionService;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageExchangeController : ControllerBase
    {
        private readonly ILogger<MessageExchangeController> _logger;
        private readonly IMessageRepository _messageRepository;
        private readonly ISocketConnectionService _socketService;

        public MessageExchangeController(
            ILogger<MessageExchangeController> logger,
            IMessageRepository messageRepository,
            ISocketConnectionService socketConnectionHandler
        )
        {
            _logger = logger;
            _messageRepository = messageRepository;
            _socketService = socketConnectionHandler;
        }

        /// <summary>
        /// Addes new message to DB and sends it to all connected clients.
        /// </summary>
        /// <param name="model"></param>
        [HttpPost("SendMessage")]
        public async Task SendMessage(SendMessageModel model)
        {
            var message = new ApiMessage()
            {
                Date = DateTime.Now,
                Text = model.Text,
                Index = model.Index
            };

            _messageRepository.AddMessage(message);
            var clientCount = await _socketService.SendMessage(message);

            _logger.LogInformation("Added message {message} with index {index} ans sent to {amount} clients", message.Text, message.Index, clientCount);
        }

        /// <summary>
        /// Retrieves messages between two Dates. StartDate and EndDate are inclusive.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>List of messages in given timespan</returns>
        [HttpGet("GetMessagesByDate")]
        public IEnumerable<ApiMessage> GetMessagesByDate([FromQuery] GetMessagesModel model)
        {
            var messages = _messageRepository.GetMessages(model);
            _logger.LogInformation("Returned messages from {start} to {end}. Amount of messages: {count}", model.StartDate, model.EndDate, messages.Count());
            return messages;
        }

        /// <summary>
        /// Starts socket connection and subscribes client to messages.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Connect")]
        public async Task Connect()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                _logger.LogDebug("Accepted new connection {}", HttpContext.Connection.Id);
                await _socketService.HandleConnection(webSocket);
                _logger.LogDebug("Connection ended {}", HttpContext.Connection.Id);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
    }
}
