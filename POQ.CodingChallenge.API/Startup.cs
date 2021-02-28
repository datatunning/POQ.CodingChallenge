// <copyright file="Startup.cs" company="Bruno DUVAL">
// Copyright (c) Bruno DUVAL.</copyright>

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using POQ.CodingChallenge.API.Services;
using POQ.CodingChallenge.API.Stores;

namespace POQ.CodingChallenge.API
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public static string CorsPolicyName = "POQCodingChallengeOriginPolicy";
        public static string SwaggerEndpointVersion = "v1";
        public static string SwaggerEndpointName = "POQ Coding Challenge API";
        public static string SwaggerEndpoint = $"/swagger/{SwaggerEndpointVersion}/swagger.json";
        public static string HealthEndpoint = "/health";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add API controllers
            services.AddControllers();

            // Add extra service for Diagnostic & Performance
            services.AddHealthChecks();
            services.AddLogging();
            services.AddMemoryCache();

            // Add basic Security & Regulation support
            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicyName, builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            // Add API Online help.
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(SwaggerEndpointVersion,
                    new OpenApiInfo {Title = SwaggerEndpointName, Version = SwaggerEndpointVersion});
                c.EnableAnnotations();
            });

            // Setup DI.
            services.AddSingleton<IProductStore, ProductStore>();
            services.AddSingleton<IProductService, ProductService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseCors(CorsPolicyName);

            app.UseRouting();

            app.UseAuthorization();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(SwaggerEndpoint, SwaggerEndpointName);
                c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks(HealthEndpoint);
                endpoints.MapSwagger();
            });
        }
    }
}