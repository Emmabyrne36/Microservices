using CommandService.Models;
using CommandService.SyncDataServices.Grpc;

namespace CommandService.Data
{
    public class SeedData
    {
        public static async Task SeedPopulation(IApplicationBuilder builder)
        {
            using var serviceScope = builder.ApplicationServices.CreateScope();
            var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();
            var platforms = grpcClient.ReturnAllPlatforms();

            await Seed(serviceScope.ServiceProvider.GetService<ICommandRepository>(), platforms);
        }

        private static async Task Seed(ICommandRepository repo, IEnumerable<Platform> platforms)
        {
            Console.WriteLine("Seeding new platforms...");
            ;

            foreach (var platform in platforms)
            {
                if (!await repo.ExternalPlatformExists(platform.ExternalId))
                {
                    await repo.CreatePlatform(platform);
                    await repo.SaveChanges();
                }
            }
        }
    }
}
