using CommandService.Models;
using CommandService.SyncDataService;

namespace CommandService.Data
{
    public class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();
            var platforms = grpcClient.ReturnAllPlatforms();

           SeedData(serviceScope.ServiceProvider.GetService<ICommandRepo>(), platforms);
        }

        public static void SeedData(ICommandRepo repo, IEnumerable<Platform> platforms)
        {
            Console.WriteLine("--> Seeding database");

            foreach (var platform in platforms)
            {
                if(!repo.ExternalPlatformIdExist(platform.ExternalId))
                {
                    repo.CreatePlatform(platform);
                }

                repo.SaveChanges();
            }

        }
    }
}
