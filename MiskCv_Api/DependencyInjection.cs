using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;

namespace MiskCv_Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMiskCVServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            var connectionString = configuration.GetConnectionString("IdentityConnString") ?? throw new InvalidOperationException("Connection string 'IdentityConnString' not found.");

            services.AddDbContext<MiskIdentityDbContext>(options => options.UseSqlServer(connectionString));           

            services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedEmail = false;

                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireUppercase = false;

                options.User.RequireUniqueEmail = false;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<MiskIdentityDbContext>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        RoleClaimType = ClaimConstants.Role,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
                        
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnForbidden = ctx =>
                        {
                            ctx.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                            return Task.CompletedTask;
                        }
                    };
                });

            //TODO: uncomment when client is done
            //services.AddCors();

            services.AddAuthorization();

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddLogging(logging =>
            {
                logging.AddDebug();
                logging.AddConsole();
                logging.AddEventSourceLogger();
            });

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

            services.AddSingleton<IDistributedCachingService, DistributedCachingService>();
            services.AddSingleton<IJwtService, JwtService>();

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IAddressRepository, AddressRepository>();
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<ISkillRepository, SkillRepository>();

            return services;
        }
    }
}
