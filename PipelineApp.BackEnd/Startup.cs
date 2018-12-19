// <copyright file="Startup.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd
{
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Reflection;
    using AutoMapper;
    using Infrastructure.Providers;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Models.Configuration;
    using Swashbuckle.AspNetCore.Swagger;

    /// <summary>
    /// .NET Core application startup class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private IConfiguration Configuration { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configures services and dependency injection for the application container.
        /// </summary>
        /// <param name="services">The application service collection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            var domain = $"https://{Configuration["Auth:Domain"]}/";
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = Configuration["Auth:ApiIdentifier"];
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v3", new Info { Title = "PipelineApp", Version = "v1" });
                c.IncludeXmlComments(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/PipelineApp.xml");
            });
            services.AddOptions();
            services.Configure<AppSettings>(Configuration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<GlobalExceptionHandlerAttribute>();
            services.AddScoped<DisableDuringMaintenanceFilterAttribute>();
            services.AddCors();
            services.AddMvc();
            services.AddAutoMapper();
        }

        /// <summary>
        /// Configures the application's HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The hosting environment.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v3/swagger.json", "Pipeline V1");
                c.RoutePrefix = "docs";
                c.DocumentTitle = "Pipeline";
                c.InjectStylesheet("/docs/custom.css");
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseCors(builder =>
                builder.WithOrigins(Configuration["Cors:CorsUrl"].Split(',')).AllowAnyHeader().AllowAnyMethod());
            app.UseMvc();
        }
    }
}
