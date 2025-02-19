
namespace PatientManagement.Api.Extensions
{
    using Serilog;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Identity;
    using Middlewares;
    using Infrastructure.Seeder;
    using Infrastructure.Entities;
    using Infrastructure.DbContexts;
    using PatientManagement.Infrastructure.Services.Interfaces;

    public static class RequestPipeline
    {
        public static async Task ConfigureRequestPipeline(this WebApplication app)
        {
            await Seed(app);
            app.UseSerilogRequestLogging();
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
                //options.SwaggerEndpoint("/swagger/v2/swagger.json", "V2");
            });

            if (!app.Environment.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseMiddleware<AuthenticationDebugMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers(); 
        }

        public static async Task Seed(WebApplication app)
        {
           using (var scope = app.Services.CreateScope())
           {
                var services = scope.ServiceProvider;
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var dbContext = services.GetRequiredService<AppDbContext>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
                var encryptionService = services.GetRequiredService<IEncryptionService>();

                if (!await dbContext.ApplicationUsers.AnyAsync())
                {
                    await SeedData.Seed(userManager, dbContext, roleManager, encryptionService);
                    await dbContext.SaveChangesAsync();
                }
           }
        }
    }
}
