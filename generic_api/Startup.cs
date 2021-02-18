using generic_api.Data;
using generic_api.Managers;
using generic_api.Model.Auth;
using generic_api.Model.ConfigurationSections;
using generic_api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Text;

namespace generic_api
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
            ConfigureAppSettings(services);
            InitialiseServices(services);
            InitializeRepositories(services);
            InitializeManagers(services);

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "generic_api", Version = "v1" });
                c.IncludeXmlComments(string.Format(@"{0}\generic_api.XML", System.AppDomain.CurrentDomain.BaseDirectory));
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                        },
                        new List<string>()
                    }
                });
            });

            services.AddDbContext<ApplicationAuthDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("default")));

            services.AddIdentity<AppIdentity, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationAuthDataContext>();
                //.AddDefaultTokenProviders();


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters() 
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSettings:Secret"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true
                    //ValidAudience = Configuration["JwtSettings:Audience"],
                    //ValidIssuer = Configuration["JwtSettings:Issuer"],
                };
            });

            InitializeAuthorization.RegisterAuthorizationHandlers(services);
        }

        private void ConfigureAppSettings(IServiceCollection services)
        {
            services.Configure<IpSecuritySettings>(Configuration.GetSection("IpSecuritySettings"));
            services.Configure<SpecialSettings>(Configuration.GetSection("SpecialSettings"));
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
        }

        private void InitialiseServices(IServiceCollection services)
        {
            services.AddSingleton<ITokenService, TokenService>();
        }

        private void InitializeRepositories(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>(r => new UserRepository(Configuration.GetConnectionString("default")));
        }

        private void InitializeManagers(IServiceCollection services)
        {
            services.AddScoped<IUsersManager, UsersManager>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "generic_api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
