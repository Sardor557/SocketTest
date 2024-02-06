using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocketTest.Database;
using SocketTest.Models;
using System.Threading.Tasks;
using System;
using SocketTest.Shared.Models;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using SocketTest.Shared.Utils;

namespace SocketTest.Repository.Services
{
    public interface IUserService
    {
        Task<viUser> AuthenticateAsync(viAuthenticateModel model);
        ValueTask<tbUser[]> GetUsersAsync();
    }

    public sealed class UserService : IUserService
    {
        private readonly MyDbContext db;
        private readonly ILogger<UserService> _logger;
        private readonly IConfiguration conf;

        public UserService(MyDbContext db, ILogger<UserService> logger, IConfiguration conf)
        {
            this.db = db;
            _logger = logger;
            this.conf = conf;
        }

        public async Task<viUser> AuthenticateAsync(viAuthenticateModel model)
        {
            var hash = CHash.EncryptMD5(model.Password);
            var res = await db.tbUsers
                              .AsNoTracking()
                              .FirstOrDefaultAsync(x => x.Login == model.Login && x.Password == hash);

            if (res == null)
            {
                _logger.LogError($"Пользователь не найден {model}");
                return new viUser();
            }

            return GetToken(res);
        }

        private viUser GetToken(tbUser res)
        {
            var SecretStr = conf["Vars:PrivateKeyString"];
            var key = Encoding.ASCII.GetBytes(SecretStr);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                           {
                               new Claim(ClaimTypes.Sid, res.Id.ToString()),
                               new Claim(ClaimTypes.Name, $"{res.Login}"),
                           }),
                Expires = DateTime.Now.AddDays(365),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var usr = new viUser();
            usr.Id = res.Id;
            usr.Token = tokenHandler.WriteToken(token);

            return usr;
        }

        public async ValueTask<tbUser[]> GetUsersAsync() => await db.tbUsers.AsNoTracking().ToArrayAsync();
    }
}
