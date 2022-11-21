using CommandService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandService.Data
{
    public class CommandRepository : ICommandRepository
    {
        private readonly AppDbContext _context;

        public CommandRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateCommand(int platformId, Command command)
        {
            if (command == null) 
            {
                throw new ArgumentNullException(nameof(command));
            }

            command.PlatformId = platformId;
            await _context.Commands.AddAsync(command);
        }

        public async Task CreatePlatform(Platform platform)
        {
            if (platform == null)
            {
                throw new ArgumentNullException(nameof(platform));
            }

            await _context.Platforms.AddAsync(platform);
        }

        public async Task<IEnumerable<Platform>> GetAllPlatforms() =>
            await _context.Platforms.ToListAsync();

        public async Task<Command> GetCommand(int platformId, int commandId) =>
            await _context.Commands.Where(c => c.PlatformId == platformId && c.Id == commandId).FirstOrDefaultAsync();

        public async Task<IEnumerable<Command>> GetCommandsForPlatform(int platformId) =>
            await _context.Commands.Where(c => c.PlatformId == platformId)
                .OrderBy(c => c.Platform.Name)
                .ToListAsync();

        public async Task<bool> PlatformExists(int platformId) =>
            await _context.Platforms.AnyAsync(p => p.Id == platformId);

        public async Task<bool> ExternalPlatformExists(int externalPlatformId) =>
            await _context.Platforms.AnyAsync(p => p.ExternalId == externalPlatformId);

        public async Task<bool> SaveChanges() =>
            await _context.SaveChangesAsync() >= 0;
    }
}
