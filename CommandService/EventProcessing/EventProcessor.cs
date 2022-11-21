using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using System.Text.Json;

namespace CommandService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            if (eventType.Equals(EventType.PlatformPublished))
            {
                await AddPlatform(message);
            }
        }

        private static EventType DetermineEvent(string notificationMessage)
        {
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

            return eventType.Event switch
            {
                "Platform Published" => EventType.PlatformPublished,
                _ => EventType.Undetermined
            };
        }

        private async Task AddPlatform(string platformPublishedMessage)
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ICommandRepository>();
            var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishDto>(platformPublishedMessage);

            try
            {
                var platform = _mapper.Map<Platform>(platformPublishedDto);

                if (!await repo.ExternalPlatformExists(platform.ExternalId))
                {
                    await repo.CreatePlatform(platform);
                    await repo.SaveChanges();

                    Console.WriteLine("--> Message received. Creating platform.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not add Platform to DB {ex.Message}");
            }
        }
    }

    enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}
