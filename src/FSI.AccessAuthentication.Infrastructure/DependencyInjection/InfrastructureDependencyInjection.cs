using FSI.AccessAuthentication.Application.Interfaces;
using FSI.AccessAuthentication.Domain.Interfaces;
using FSI.AccessAuthentication.Infrastructure.Context;
using FSI.AccessAuthentication.Infrastructure.Interfaces;
using FSI.AccessAuthentication.Infrastructure.Messaging;
using FSI.AccessAuthentication.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FSI.AccessAuthentication.Infrastructure.DependencyInjection
{
    public static class InfrastructureDependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDbContext, DapperDbContext>();

            // Repositórios
            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            services.AddScoped<IProfileRepository, ProfileRepository>();
            services.AddScoped<ISystemRepository, SystemRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMessagingRepository, MessagingRepository>();


            // RabbitMQ Publisher
            services.AddSingleton<IMessageQueuePublisher, RabbitMqPublisher>();
        }
    }
}
