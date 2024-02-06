using Microsoft.Extensions.Logging;
using SocketTest.Database;

namespace SocketTest.Repository.Services
{
    public interface IUserService
    {

    }

    public sealed class UserService : IUserService
    {
        private readonly MyDbContext db;
        private readonly ILogger<UserService> _logger;

        public UserService(MyDbContext db, ILogger<UserService> logger)
        {
            this.db = db;
            _logger = logger;
        }
    }
}
