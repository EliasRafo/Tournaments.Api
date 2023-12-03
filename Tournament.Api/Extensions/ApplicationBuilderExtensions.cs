using System;
using Tournament.Data.Data;

namespace Tournament.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task SeedDataAsync(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var db = serviceProvider.GetRequiredService<TournamentApiContext>();

                //db.Database.EnsureDeleted();
                //db.Database.Migrate();

                try
                {
                    await SeedData.InitAsync(db);
                }
                catch (Exception e)
                {
                    throw;
                }
            }

        }
    }
}
