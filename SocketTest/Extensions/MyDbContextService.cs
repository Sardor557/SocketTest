using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocketTest.Database;

namespace SocketTest.Extensions
{
    public static class MyDbContextService
    {
        public static void AddMyDbContext(this IServiceCollection services, IConfiguration conf)
        {

            services.AddDbContext<MyDbContext>(opt => opt.UseNpgsql(conf.GetConnectionString("WantedConnStr"))
                                                .UseUpperSnakeCaseNamingConvention()
                                                .EnableDetailedErrors()
                                                .EnableSensitiveDataLogging()
                                            );
        }

        public static void UpdateMigrateDatabase(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<MyDbContext>())
                {
                    context.Database.Migrate();
                }
            }
        }

    }
}
