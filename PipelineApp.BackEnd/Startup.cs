// <copyright file="Startup.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Reflection;
    using System.Text;
    using AutoMapper;
    using Infrastructure.Data;
    using Infrastructure.Data.Entities;
    using Infrastructure.Data.Repositories;
    using Infrastructure.Providers;
    using Infrastructure.Seeders;
    using Infrastructure.Services;
    using Interfaces;
    using Interfaces.Repositories;
    using Interfaces.Services;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using Models.Configuration;
    using Neo4j.Driver.V1;
    using Neo4jClient;
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

            services.AddTransient<IUserStore<UserEntity>, UserRepository>();
            services.AddTransient<IRoleStore<RoleEntity>, RoleRepository>();
            services.AddIdentity<UserEntity, RoleEntity>(options => { options.User.AllowedUserNameCharacters = string.Empty; })
                .AddDefaultTokenProviders();
            services.AddAuthentication(options => { options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = Configuration["Auth:Issuer"],
                        ValidAudience = Configuration["Auth:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Auth:Key"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v3", new Info { Title = "PipelineApp", Version = "v1" });
                c.IncludeXmlComments(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/PipelineApp.xml");
            });
            services.AddOptions();
            services.Configure<AppSettings>(Configuration);

            services.AddTransient<DatabaseSeeder>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<GlobalExceptionHandlerAttribute>();
            services.AddScoped<DisableDuringMaintenanceFilterAttribute>();
            services.AddSingleton(provider => new GraphClient(new Uri(Configuration["GraphDb:Hostname"]), Configuration["GraphDb:Username"], Configuration["GraphDb:Password"]));

            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IFandomRepository, FandomRepository>();
            services.AddSingleton<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IFandomService, FandomService>();
            services.AddScoped<IPersonaService, PersonaService>();

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
