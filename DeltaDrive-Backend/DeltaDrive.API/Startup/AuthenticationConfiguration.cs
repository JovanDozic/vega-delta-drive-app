using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DeltaDrive.API.Startup
{
    public static class AuthenticationConfiguration
    {
        private static IConfiguration? _config;

        public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration config)
        {
            _config = config;
            AddAuthentication(services);
            AddAuthorizationPolicies(services);
            return services;
        }

        private static void AddAuthentication(IServiceCollection services)
        {
            if (_config is null) throw new InvalidOperationException("Configuration is not set.");
            var key = _config["Jwt:Key"];
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };
                options.Events = new()
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("AuthenticationTokens-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        }

        private static void AddAuthorizationPolicies(IServiceCollection services)
        {
            // TODO: Add policies here

            //services.AddAuthorizationBuilder()
            //    .AddPolicy("adminPolicy", policy => policy.RequireRole("admin"));

            ////.AddPolicy("storePolicy", policy => policy.RequireRole("store"));
            ////.AddPolicy("managerPolicy", policy => policy.RequireRole("manager"))
            ////.AddPolicy("servicerPolicy", policy => policy.RequireRole("servicer"));

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("adminPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "admin"));
            //});
        }
    }
}
