using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocketTest.Database;
using SocketTest.Shared.Models;
using SocketTest.Shared.Utils;
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
        private readonly MyDbContext db;
        private readonly ConcurrentDictionary<int, WebSocket> _clients = new ConcurrentDictionary<int, WebSocket>();

        public WebSocketHandler(IChatService chatService, ILogger<WebSocketHandler> logger, MyDbContext db)
        {
            this.chatService = chatService;
            this._logger = logger;
            this.db = db;
        }

        public async Task HandleWebSocket(HttpContext context, WebSocket webSocket)
        {
            try
            {
                int senderId = int.Parse(context.User.FindFirst("Id").Value);
                _clients.TryAdd(senderId, webSocket);

                try
                {
                    while (webSocket.State == WebSocketState.Open)
                    {
                        var buffer = new byte[1024 * 4];
                        var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
                            break;
                        }

                        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                        // Определяем получателя сообщения
                        var model = message.FromJson<ChatModel>();
                        await SendMessageAsync(model);
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
                    _clients.TryRemove(senderId, out dummy);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("WebSocketHandler.HandleWebSocket main error: {0}", ex.Message);
            }
        }

        private async Task SendMessageAsync(ChatModel chatModel)
        {
            if (_clients.TryGetValue(chatModel.UserId.Value, out var recipientWebSocket))
            {
                var buffer = Encoding.UTF8.GetBytes(chatModel.Message);
                await recipientWebSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            else
            {
                // Получатель не подключен, обработка этой ситуации
            }
        }
    }
}
