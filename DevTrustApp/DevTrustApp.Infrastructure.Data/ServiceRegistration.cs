using DevTrustApp.Core.ApplicationService.Implementation;
using DevTrustApp.Core.ApplicationService.Interface;
using DevTrustApp.Core.DomainService;
using DevTrustApp.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;

namespace DevTrustApp.Infrastructure.Data
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices (this IServiceCollection services, IConfiguration configuration)
        {
            var defaultConnectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DevTrustAppDbContext>(options =>options.UseSqlServer(defaultConnectionString));
            services.AddScoped<IPersonReadRepository, PersonReadRepository>();
            services.AddScoped<IPersonWriteRepository, PersonWriteRepository>();
            services.AddScoped<IAddressWriteRepository, AddressWriteRepository>();
            services.AddScoped<IAddressReadRepository, AddressReadRepository>();
            services.AddScoped<IPersonService, PersonService>();
            return services;
        }
    }
}
