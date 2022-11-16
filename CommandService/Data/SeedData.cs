using CommandService.Models;
using CommandService.SyncDataServices.Grpc;

namespace CommandService.Data
{
    public class SeedData
    {
        public static void SeedPopulation(IApplicationBuilder builder)
        {
            using var serviceScope = builder.ApplicationServices.CreateScope();
            var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();
            var platforms = grpcClient.ReturnAllPlatforms();

            Seed(serviceScope.ServiceProvider.GetService<ICommandRepository>(), platforms);
        }

        private static void Seed(ICommandRepository repo, IEnumerable<Platform> platforms)
        {
            Console.WriteLine("Seeding new platforms...");
            ;

            foreach (var platform in platforms)
            {
                if (!repo.ExternalPlatformExists(platform.ExternalId))
                {
                    repo.CreatePlatform(platform);
                    repo.SaveChanges();
                }
            }
        }
    }
}
