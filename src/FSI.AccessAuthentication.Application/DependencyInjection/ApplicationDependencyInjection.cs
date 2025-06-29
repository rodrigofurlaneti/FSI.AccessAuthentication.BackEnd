using FSI.AccessAuthentication.Application.Interfaces;
using FSI.AccessAuthentication.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FSI.AccessAuthentication.Application.DependencyInjection
{
    public static class ApplicationDependencyInjection
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationAppService, AuthenticationAppService>();
            services.AddScoped<IProfileAppService, ProfileAppService>();
            services.AddScoped<ISystemAppService, SystemAppService>();
            services.AddScoped<IUserAppService, UserAppService>();
            services.AddScoped<IMessagingAppService, MessagingAppService>();
        }
    }
}
