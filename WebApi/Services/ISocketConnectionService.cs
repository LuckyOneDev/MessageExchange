using System.Net.WebSockets;
using WebApi.DAL;

namespace WebApi.Services
{
    public interface ISocketConnectionService
    {
        /// <summary>
        /// Handles connection and adds it to list of connected clients.
        /// Resolves when client disconnects.
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public Task HandleConnection(WebSocket socket);

        /// <summary>
        /// Sends message to all connected clients.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Amount of clients that were able to recieve message</returns>
        public Task<int> SendMessage(ApiMessage message);

        /// <summary>
        /// Returns amount of clients that are connected.
        /// </summary>
        /// <returns></returns>
        public int GetClientsCount();
    }
}
