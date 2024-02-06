using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketTest.Repository.Services
{
    public interface IChatService
    {
        Task SaveMessageAsync(string message);
        Task<string[]> GetMessagesAsync();
    }

    public sealed class ChatService : IChatService
    {
        public Task<string[]> GetMessagesAsync()
        {
            throw new NotImplementedException();
        }

        public Task SaveMessageAsync(string message)
        {
            throw new NotImplementedException();
        }

        private async Task SendPrimitive(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result is not null)
            {
                while (!result.CloseStatus.HasValue)
                {
                    string msg = Encoding.UTF8.GetString(new ArraySegment<byte>(buffer, 0, result.Count));
                    await Console.Out.WriteLineAsync($"client says: {msg}");
                    await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes($"Server says: {DateTime.Now} - Hi!")),
                        result.MessageType, result.EndOfMessage, CancellationToken.None);

                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);                    
                }
            }
        }

    }
}
