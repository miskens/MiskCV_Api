using System.Reflection;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using MiskCv_Api.Mapping;

namespace MiskCv_Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMiskCVServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(configuration.GetSection("AzureAd"));

            services.AddControllers();
            
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddMappings();
            //services.AddSingleton<IMapper, Mapper>();

            services.AddDbContext<MiskCvDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("MiskCvDbTEMPConnString"));
            });

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IAddressRepository, AddressRepository>();
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<ISkillRepository, SkillRepository>();

            return services;
        }
    }
}
