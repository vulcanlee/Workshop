using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOBCore.DataAccesses;
using LOBCore.DTOs;
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
                     ValidateIssuerSigningKey = true,
                     ValidIssuer = Configuration["Tokens:ValidIssuer"],
                     ValidAudience = Configuration["Tokens:ValidAudience"],
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:IssuerSigningKey"])),
                     RequireExpirationTime = true,
                 };
                 options.Events = new JwtBearerEvents()
                 {
                     //OnChallenge = context =>
                     //{
                     //    return Task.CompletedTask;
                     //},
                     //OnMessageReceived = context =>
                     //{
                     //    return Task.CompletedTask;
                     //},
                     //OnAuthenticationFailed = context =>
                     //{
                     //    context.NoResult();

                     //    context.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "JWT token is incorrectly formatted";
                     //    return Task.CompletedTask;
                     //},
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

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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
