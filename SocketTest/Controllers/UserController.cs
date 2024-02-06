using Microsoft.AspNetCore.Mvc;
using SocketTest.Models;
using SocketTest.Repository.Services;
using SocketTest.Shared.Models;
using System.Threading.Tasks;

namespace SocketTest.Controllers
{    
    [ApiVersion("1.0")]
    [Route("[controller]")]

    public class UserController : ControllerBase
    {
        private readonly IUserService service;

        public UserController(IUserService service)
        {
            this.service = service;
        }

        [HttpPost("login")]
        public Task<viUser> AuthenticateAsync([FromBody] viAuthenticateModel model)
        {          
            return service.AuthenticateAsync(model);
        }

        [HttpGet]
        public ValueTask<tbUser[]> GetUsersAsync() => service.GetUsersAsync();
    }
}
