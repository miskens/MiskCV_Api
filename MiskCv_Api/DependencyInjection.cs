using System.Reflection;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using MiskCv_Api.Mapping;
using MiskCv_Api.Services.Repositories.IdentityUserRepository;
using MiskCv_Api.Services.AzureServices;
using MiskCv_Api.Extensions.DistributedCache;
using MiskCv_Api.Services;

namespace MiskCv_Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMiskCVServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            var connectionString = configuration.GetConnectionString("IdentityConnString") ?? throw new InvalidOperationException("Connection string 'IdentityConnString' not found.");

            services.AddDbContext<MiskIdentityDbContext>(options => options.UseSqlServer(connectionString));

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<MiskIdentityDbContext>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(configuration.GetSection("AzureAd"));

            //services.AddMvc();
            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // DI from Mapping folder
            services.AddMappings();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("RedisMiskConnString");
                options.InstanceName = "MiskCv_";
            });

            services.AddDbContext<MiskCvDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("MiskCvDbTEMPConnString"));
            });

            services.AddSingleton<IAzureAuthenticationService, AzureAuthenticationService>();
            services.AddSingleton<IJwtService>(new JwtService(
                                                configuration["Jwt:Key"]!,
                                                configuration["Jwt:Issuer"]!,
                                                configuration["Jwt:Audience"]!));
            services.AddScoped<IUserManager, UserManager>();

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IAddressRepository, AddressRepository>();
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<ISkillRepository, SkillRepository>();

            // Static class
            DistributedCacheExtension.SetConfiguration(configuration);

            return services;
        }
    }
}
