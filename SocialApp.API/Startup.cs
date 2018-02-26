using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SocialApp.API.Data;
using SocialApp.API.Helpers;

namespace SocialApp.API
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
            // Use SQL Database if in Azure (Production), otherwise, use local SQL Server
            if(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                services.AddDbContext<Data.DataContext>(options =>
                        options.UseSqlServer(Configuration.GetConnectionString("Default")));

                // Automatically perform database migration
                services.BuildServiceProvider().GetService<Data.DataContext>().Database.Migrate();
            }
            else
            {
                services.AddDbContext<Data.DataContext>(options => 
                    options.UseSqlServer(Configuration.GetConnectionString("LocalSqlServer")));
            }

            services.AddTransient<SeedData>();

            //services.AddMvc();
            // based on "Step 2: Authorize all the things" 
            // from: https://github.com/blowdart/AspNetAuthorizationWorkshop
            // Want to apply the "Authorize" attribute to all controllers by default
            // and only allow public access to controllers that have the
            // "AllowAnonymous" attribute.
            // If in future you need different authorization in different parts, see
            // https://joonasw.net/view/apply-authz-by-default
            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                            .RequireAuthenticatedUser()
                            .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            }).AddJsonOptions(options => {
                // TODO Fix the circular referrencing then remove this!
                // In Models the User class is referencing Photos and vica versa
                options.SerializerSettings.ReferenceLoopHandling = 
                Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.AddCors();
            services.AddAutoMapper();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<ISocialAppRepository, SocialAppRepository>();

            // Add Jwt Bearer Authenticatin using a key stored in the "appsettings.json" config file.
            // This same key will be used to generate each JWT token handed to clients after they
            // log in.
            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, SeedData seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // Only allow CORS when testing locally in development mode
                app.UseCors( x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials() );
            }
            else
            {
                // Can set this environment temporarily from command line as follows
                // $env:ASPNETCORE_ENVIRONMENT = "Production"

                // Configure the global exception handler here so that you don't need 
                // to catch all the exceptions in all the controllers etc
                // Read http://www.talkingdotnet.com/global-exception-handling-in-aspnet-core-webapi/
                app.UseExceptionHandler( builder => 
                {
                    builder.Run( async context => {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        
                        if( error != null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
            }

            app.UseAuthentication();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();
            // We create a "Fallback" controller to pass non-API requests to the
            // Angular SPA (i.e. when use hits refresh). When using the "MapWhen()"
            // function, you can see we make sure it doesn't start with /api, if it does,
            // it'll 404 within .NET if it can't be found. Otherwise, it will map to
            // the "Fallback" controller that will return the SPA as html
            // See here for more infomation: 
            // https://docs.microsoft.com/en-us/aspnet/core/security/cors
            app.MapWhen(x => !x.Request.Path.Value.StartsWith("/api"), builder =>
            {
                builder.UseMvc(routes =>
                {
                    routes.MapSpaFallbackRoute(
                        name: "spa-fallback",
                        defaults: new { controller = "Fallback", action = "Index" });
                });
            });

            // Only use this SeedUsers when you need to seed more data to database
            //seeder.SeedUsers();
        }
    }
}
