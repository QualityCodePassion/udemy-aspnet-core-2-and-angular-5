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
            services.AddDbContext<Data.DataContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("Default")));

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

            // TODO Put this key into a config
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

            // TODO Only doing this for demo purpose, I wouldn't use this in productin code
            // We want to allow CORS (Cross origin requests) so that our Angular SPA can
            // easily make requests to our API to get the values it needs
            app.UseCors( x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials() );
            app.UseAuthentication();
            app.UseMvc();

            // Only use this SeedUsers when you need to seed more data to database
            //seeder.SeedUsers();
        }
    }
}
