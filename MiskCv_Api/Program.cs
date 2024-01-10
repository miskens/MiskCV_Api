using MapsterMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using MiskCv_Api.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiskCv_Api.Data;
using MiskCv_Api.Services.AzureServices;
using Microsoft.Extensions.Caching.Distributed;
using MiskCv_Api.Extensions.DistributedCache;

namespace MiskCv_Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Top level DI
            builder.Services.AddMiskCVServices(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            Task SaveAccessTokenTask = AppAccessTokenHandler.AddAppAccessTokenAsync(app);
            SaveAccessTokenTask.Wait();

            app.MapControllers();

            app.Run();
        }
    }   
}