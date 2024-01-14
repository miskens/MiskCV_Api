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
using MiskCv_Api.Services;
using Microsoft.Extensions.Caching.Distributed;
using MiskCv_Api.Services.DistributedCacheService;

namespace MiskCv_Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMiskCVServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            var connectionString = configuration.GetConnectionString("IdentityConnString") ?? throw new InvalidOperationException("Connection string 'IdentityConnString' not found.");

            services.AddDbContext<MiskIdentityDbContext>(options => options.UseSqlServer(connectionString));

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<MiskIdentityDbContext>();

            services.AddAuthentication(options =>
            {
                //options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
            })
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

            services.AddSingleton<IDistributedCachingService, DistributedCachingService>();
            services.AddSingleton<IJwtService, JwtService>();

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IAddressRepository, AddressRepository>();
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<ISkillRepository, SkillRepository>();

            // Static class
            //DistributedCacheExtension.SetConfiguration(configuration);

            return services;
        }
    }
}
