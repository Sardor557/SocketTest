using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketTest.Repository.Services
{
    public interface IWebSocketHandler
    {
        Task HandleWebSocket(HttpContext context, WebSocket webSocket);
    }

    public sealed class WebSocketHandler : IWebSocketHandler
    {
        private readonly IChatService chatService;
        private readonly ILogger<WebSocketHandler> _logger;
        private readonly ConcurrentDictionary<string, WebSocket> _clients = new ConcurrentDictionary<string, WebSocket>();

        public WebSocketHandler(IChatService chatService, ILogger<WebSocketHandler> logger)
        {
            this.chatService = chatService;
            this._logger = logger;
        }

        public async Task HandleWebSocket(HttpContext context, WebSocket webSocket)
        {
            try
            {
                int senderId = int.Parse(context.User.FindFirst("Id").Value);

                var clientId = context.Connection.Id.ToString(); // Уникальный идентификатор для каждого клиента
                _clients.TryAdd(clientId, webSocket);

                try
                {
                    while (webSocket.State == WebSocketState.Open)
                    {
                        var buffer = new byte[1024];
                        var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
                            break;
                        }

                        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                        // Определяем получателя сообщения
                        var messageParts = message.Split(':', 2); // Первая часть - идентификатор получателя, вторая - текст сообщения
                        var recipientId = messageParts[0];
                        var messageText = messageParts[1];

                        // Отправляем сообщение получателю
                        await SendMessageAsync(recipientId, messageText);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("WebSocketHandler.HandleWebSocket error: {0}", ex.Message);
                }
                finally
                {
                    // Удаляем клиента из словаря при отключении
                    WebSocket dummy;
                    _clients.TryRemove(clientId, out dummy);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("WebSocketHandler.HandleWebSocket main error: {0}", ex.Message);
            }
        }

        private async Task SendMessageAsync(string recipientId, string message)
        {
            if (_clients.TryGetValue(recipientId, out var recipientWebSocket))
            {
                var buffer = Encoding.UTF8.GetBytes(message);
                await recipientWebSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            else
            {
                // Получатель не подключен, обработка этой ситуации
            }
        }
    }
}
