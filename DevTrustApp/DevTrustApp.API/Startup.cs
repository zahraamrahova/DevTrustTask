using DevTrustApp.API.Helper;
using DevTrustApp.Core.ApplicationService.Implementation;
using DevTrustApp.Core.ApplicationService.Interface;
using DevTrustApp.Core.DomainService;
using DevTrustApp.Infrastructure.Data;
using DevTrustApp.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace DevTrustApp.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<DevTrustAppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IPersonReadRepository, PersonReadRepository>();
            services.AddScoped<IPersonWriteRepository, PersonWriteRepository>();
            services.AddScoped<IAddressWriteRepository, AddressWriteRepository>();
            services.AddScoped<IAddressReadRepository, AddressReadRepository>();
            services.AddScoped<IPersonService, PersonService>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DevTrustApp.API", Version = "v1" });
            });
            services.AddMvc(o => o.InputFormatters.Insert(0, new RawRequestBodyFormatter()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DevTrustApp.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
