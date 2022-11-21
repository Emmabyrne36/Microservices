using CommandService.Models;

namespace CommandService.Data
{
    public interface ICommandRepository
    {
        Task<bool> SaveChanges();
        
        Task<IEnumerable<Platform>> GetAllPlatforms();
        Task CreatePlatform(Platform platform);
        Task<bool> PlatformExists(int platformId);
        Task<bool> ExternalPlatformExists(int externalPlatformId);

        Task<IEnumerable<Command>> GetCommandsForPlatform(int platformId);
        Task<Command> GetCommand(int platformId, int commandId);
        Task CreateCommand(int platformId, Command command);
    }
}
