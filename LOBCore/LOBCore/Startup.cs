using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOBCore.DataAccesses;
using LOBCore.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using LOBCore.DataTransferObject.DTOs;
using LOBCore.BusinessObjects.Factories;
using Microsoft.Extensions.DependencyInjection;
using LOBCore.Filters;

namespace LOBCore
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
            services.AddTransient<APIResult>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     RequireExpirationTime = true,
                     ValidateIssuerSigningKey = true,
                     ValidIssuer = Configuration["Tokens:ValidIssuer"],
                     ValidAudience = Configuration["Tokens:ValidAudience"],
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:IssuerSigningKey"])),
                 };
                 options.Events = new JwtBearerEvents()
                 {
                     OnChallenge = context =>
                     {
                         return Task.CompletedTask;
                     },
                     OnMessageReceived = context =>
                     {
                         return Task.CompletedTask;
                     },
                     OnAuthenticationFailed = context =>
                     {
                         context.NoResult();
                         if (context.Exception != null)
                         {
                             APIResult apiResult;
                             if (context.Exception is SecurityTokenExpiredException)
                             {
                                 apiResult = APIResultFactory.Build(false, StatusCodes.Status401Unauthorized,
                                  Helpers.ErrorMessageEnum.SecurityTokenExpiredException);
                             }
                             else
                             {
                                 apiResult = APIResultFactory.Build(false, StatusCodes.Status401Unauthorized,
                                  Helpers.ErrorMessageEnum.Exception,
                                  exceptionMessage: $"({context.Exception.GetType().Name}), {context.Exception.Message}");
                             }
                             context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                             context.Response.ContentType = "application/json";
                             context.Response.WriteAsync(JsonConvert.SerializeObject(apiResult)).Wait();
                         }
                         return Task.CompletedTask;
                     },
                     OnTokenValidated = context =>
                     {
                         Console.WriteLine("OnTokenValidated: " +
                             context.SecurityToken);
                         return Task.CompletedTask;
                     }

                 };
             });

            services.AddEntityFrameworkSqlite().AddDbContext<LOBDatabaseContext>(options =>
            {
                options.UseSqlite(Configuration.GetConnectionString("MyDatabaseConnection"));
            });

            services.AddMvc(config =>
            {
                config.Filters.Add<ValidateModelAttribute>();
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            serviceProvider.GetRequiredService<LOBDatabaseContext>().Database.Migrate();

            app.UseExceptionMiddleware();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
