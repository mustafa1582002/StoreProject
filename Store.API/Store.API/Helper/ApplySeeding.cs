using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Store.Data.Entities.Context;
using Store.Data.IdentityEntities;
using Store.Repository;

namespace Store.API.Helper
{
    public class ApplySeeding
    {
        public static async Task ApplySeedingAsync(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var service = scope.ServiceProvider;

                var loggerFactory= service.GetRequiredService<ILoggerFactory>();
                try
                {
                    var context = service.GetRequiredService<StoreDbContext>();
                    var userManager = service.GetRequiredService<UserManager<AppUser>>();

                    //Check if the Migration applied or not
                    await context.Database.MigrateAsync();

                    await StoreContextSeed.SeedAsync(context, loggerFactory);
                    await AppIdentityContextSeed.SeedUserAsync(userManager);
                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<StoreContextSeed>();

                    logger.LogError(ex.Message);
                }
            }
        }

    }
}
