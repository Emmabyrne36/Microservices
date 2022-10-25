using PlatformService.Models;

namespace PlatformService.Data
{
    public static class SeedData
    {
        public static void SeedPopulation(IApplicationBuilder builder)
        {
            using (var serviceScope = builder.ApplicationServices.CreateScope())
            {
                Seed(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }

        private static void Seed(AppDbContext context)
        {
            if (!context.Platforms.Any())
            {
                Console.WriteLine("--> Seeding Data...");
                context.Platforms.AddRange(
                    new Platform() { Name = "Dot Net", Publisher = "Microsoft", Cost = "Free" },
                    new Platform() { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" },
                    new Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
                );

                context.SaveChanges();
                return;
            }

            Console.WriteLine("--> We already have data.");
        }
    }
}
