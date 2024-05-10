using DeltaDrive.DA.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DeltaDrive.API.Startup
{
    public class Startup(IConfiguration config)
    {
        private readonly IConfiguration _config = config;

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = _config.GetConnectionString("LocalhostConnectionString");

            services.AddControllers();

            services.AddEndpointsApiExplorer();

            services.ConfigureSwagger();

            services.AddDbContext<DataContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            services.ConfigureAuthentication(_config); // JWT Authentication and Authorization policies

            services.ConfigureModules(); // AutoMappers and Dependency Injection

            services.ConfigureCors("_corsPolicy"); // CORS policy
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("_corsPolicy");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
