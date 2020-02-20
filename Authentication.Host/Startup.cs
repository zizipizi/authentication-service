using System;
using System.Net.Http;
using Authentication.Data.Models;
using Authentication.Host.Healthchecks;
using Authentication.Host.Middlewares;
using Authentication.Host.Repositories;
using Authentication.Host.Repositories.RepositoryExtensions;
using Authentication.Host.Services;
using Authentication.Host.Services.ServiceExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NSV.Security.JWT;
using NSV.Security.Password;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Prometheus;
using Serilog;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Authentication.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AuthContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddUserRepository();
            services.AddTokenRepository();

            services.AddAuthService();
            services.AddUserService();
            services.AddAdminService();

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Auth",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });

            services.AddJwt();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = JwtSettings.TokenValidationParameters();
            });

            services.AddPassword(Configuration);

            var redisOptions = ConfigurationOptions.Parse(Configuration.GetConnectionString("Redis"));
            services.AddStackExchangeRedisCache(options =>
                {
                    options.ConfigurationOptions = redisOptions;
                });

            services.AddHttpClient();

            services.AddHealthChecks()
                .AddDbContextCheck<AuthContext>()
                .AddRedis(redisOptions.ToString())
                .AddPrometheus(Configuration.GetConnectionString("Prometheus"));
                //.AddElasticsearch()
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHealthChecks("/hc");

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth API V1");
                c.RoutePrefix = "";
            });

            app.UseRouting();
            app.UseHttpMetrics();
            
            app.UseRequestCounterMetrics();
            
            app.UseAuthentication();
            app.UseAuthorization();
           
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapMetrics();
            });

        }
    }
}
