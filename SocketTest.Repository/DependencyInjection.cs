using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SocketTest.Repository.Services;

namespace SocketTest.Repository
{
    public static class DependencyInjection
    {
        public static IApplicationBuilder UseWebSocketHandler(this IApplicationBuilder builder, WebSocketHandler handler)
        {
            return builder.Use(async (context, next) =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    await handler.HandleWebSocket(context, webSocket);
                }
                else
                {
                    await next();
                }
            });
        }

        public static void AddChatService(this IServiceCollection services)
        {
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<IWebSocketHandler, WebSocketHandler>();
        }
    }
}
