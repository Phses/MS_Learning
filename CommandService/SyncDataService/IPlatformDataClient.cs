using CommandService.Models;

namespace CommandService.SyncDataService
{
    public interface IPlatformDataClient
    {
        IEnumerable<Platform> ReturnAllPlatforms();
    }
}
