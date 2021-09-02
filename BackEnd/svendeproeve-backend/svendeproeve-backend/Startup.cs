using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using svendeproeve_backend.Data;
using Microsoft.EntityFrameworkCore;
using svendeproeve_backend.Hubs;
using svendeproeve_backend.Models;
using Microsoft.AspNetCore.Identity;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using svendeproeve_backend.Models.User;

namespace svendeproeve_backend
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
            services.AddDbContext<Databasedcontext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("DatabaseProduction"));
            });
            services.AddControllers();
            services.AddSignalR();

            services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<Databasedcontext>().AddTokenProvider(Configuration["Jwt:Provider"], typeof(DataProtectorTokenProvider<AppUser>));

            var key = Encoding.ASCII.GetBytes(Configuration["Jwt:Key"]);

            services.AddAuthorization();

            services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {

                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddCors(option => option.AddPolicy("AllowAll", builder =>
            {
                builder.WithOrigins("https://*", "http://*")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .SetIsOriginAllowed((host) => true);
            }));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<productsHub>("/products");
                endpoints.MapControllers();
            });
        }
    }
}
