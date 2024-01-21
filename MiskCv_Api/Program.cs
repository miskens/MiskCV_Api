using Microsoft.AspNetCore.Identity;

namespace MiskCv_Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddMiskCVServices(builder.Configuration);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.Use(async (context, next) =>
            {
                var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                await next.Invoke();
                logger.LogInformation($"Request finished with status code: {context.Response.StatusCode}");
            });

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            //TODO: uncomment and change url when client is done
            //app.UseCors(
            //    options => options.WithOrigins("http://MiskCV...").AllowAnyMethod()
            //);

            SeedRolesAndAdmin.Seed(app, builder);

            app.Run();
        }
    }   
}