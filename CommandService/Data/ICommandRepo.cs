using CommandService.Models;

namespace CommandService.Data
{
    public interface ICommandRepo
    {
        bool SaveChanges();

        //Platform
        IEnumerable<Platform> GetAllPlatforms();
        Platform GetPlatform(int platformId);
        bool PlatformExist(int platformId); 
        bool ExternalPlatformIdExist(int externalPlatformId); 
        void CreatePlatform(Platform platform);

        //Command
        IEnumerable<Command> GetCommandsForPlatform(int  platformId);
        Command GetCommand(int  platformId, int commandId);
        void CreateCommand(int platformId, Command command);

    }
}
