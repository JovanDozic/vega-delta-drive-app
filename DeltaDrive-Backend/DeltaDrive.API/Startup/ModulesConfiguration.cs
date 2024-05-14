using DeltaDrive.API.Helpers;
using DeltaDrive.BL.Contracts.IService;
using DeltaDrive.BL.Contracts.IService.Authentication;
using DeltaDrive.BL.Mapper;
using DeltaDrive.BL.Service;
using DeltaDrive.BL.Service.Authentication;
using DeltaDrive.DA;
using DeltaDrive.DA.Contracts;

namespace DeltaDrive.API.Startup
{
    public static class ModulesConfiguration
    {
        public static IServiceCollection ConfigureModules(this IServiceCollection services)
        {
            ConfigureServices(services);
            ConfigureAutoMappers(services);
            return services;
        }

        // [DEPENDENCY INJECTION]
        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<IVehicleBookingService, VehicleBookingService>();

            services.AddSingleton<IRideSimulationUpdater, RideSimulationUpdater>();
            services.AddHostedService<RideSimulationService>();
            services.AddScoped<IRideSimulationService, RideSimulationService>();
        }

        // [AUTO MAPPERS]
        private static void ConfigureAutoMappers(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(CommonProfile));
        }
    }
}
