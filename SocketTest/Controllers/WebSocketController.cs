using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System;
using System.Threading.Tasks;
using SocketTest.Repository.Services;
using Microsoft.AspNetCore.Authorization;

namespace SocketTest.Controllers
{
    public class WebSocketController : ControllerBase
    {   
        private readonly IWebSocketHandler _handler;

        public WebSocketController(IWebSocketHandler handler)
        {
            _handler = handler;
        }

        [Authorize]
        [Route("/ws")]
        public async Task ChatAsync()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await _handler.HandleWebSocket(HttpContext, webSocket);
            }
            else
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}
