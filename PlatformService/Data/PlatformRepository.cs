using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public class PlatformRepository : IPlatformRepository
    {
        private readonly AppDbContext _context;

        public PlatformRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreatePlatform(Platform platform)
        {
            if (platform == null)
            {
                throw new ArgumentNullException(nameof(platform));
            }

            await _context.Platforms.AddAsync(platform);
        }

        public async Task<IEnumerable<Platform>> GetAllPlatforms() => await _context.Platforms.ToListAsync();

        public async Task<Platform> GetPlatformById(int id) => await _context.Platforms.FirstOrDefaultAsync(p => p.Id == id);

        public async Task<bool> SaveChanges() => await _context.SaveChangesAsync() >= 0;
    }
}
