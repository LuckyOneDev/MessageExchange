using System.Net.WebSockets;
using System.Text.Json;
using WebApi.DAL;

namespace WebApi.Services
{
    public class SocketConnectionService : ISocketConnectionService
    {
        private readonly List<WebSocket> _clients = new();
        private readonly ILogger<ISocketConnectionService> _logger;
        public SocketConnectionService(ILogger<ISocketConnectionService> logger)
        {
            _logger = logger;
        }

        public int GetClientsCount()
        {
            return _clients.Count;
        }

        public async Task HandleConnection(WebSocket socket)
        {
            _clients.Add(socket);
            var buffer = new byte[1024];
            _logger.LogInformation("Currently active connections: {}", _clients.Count);
            var result = await socket.ReceiveAsync(buffer, CancellationToken.None);
            // When anything is recieved we close the connection.
            _clients.Remove(socket);
            _logger.LogInformation("Currently active connections: {}", _clients.Count);
        }

        public async Task<int> SendMessage(ApiMessage message)
        {
            var serializedMessage = JsonSerializer.SerializeToUtf8Bytes(message);
            for (var i = 0; i < _clients.Count; i++)
            {
                var client = _clients[i];
                // If we can't send message to client we remove it from list.
                try
                {
                    await client.SendAsync(
                        serializedMessage,
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None
                    );
                } catch (Exception e)
                {
                    _clients.Remove(client);
                    i--;
                }
            }

            _logger.LogInformation("Currently active connections: {}", _clients.Count);
            return _clients.Count;
        }
    }
}
