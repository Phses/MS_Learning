using PlatformService.Dtos;

namespace PlatformService.SyncDataService.http
{
    public interface IPlatformDataClient
    {
        Task SendPlatformToCommandService(PlatformReadDto platformCreateDto);
    }
}
