using Microsoft.Extensions.Logging;
using SocketTest.Database;
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
        private readonly ILogger<ChatService> _logger;
        private readonly MyDbContext db;

        public ChatService(MyDbContext db, ILogger<ChatService> logger)
        {
            this.db = db;
            _logger = logger;
        }

        public Task<string[]> GetMessagesAsync()
        {
            throw new NotImplementedException();
        }

        public Task SaveMessageAsync(string message)
        {
            throw new NotImplementedException();
        }
    }
}
