using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using System.Text.Json;

namespace CommandService.EventProcessing
{
    public class EventProcessing : IEventProcessing
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessing(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }
        public void EventProcess(string message)
        {
            var eventType = DetermineEvent(message);

            if (eventType == EventType.PlatformPublished)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

                    var publishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(message);
                    try
                    {
                        if (!repo.ExternalPlatformIdExist(publishedDto.Id))
                        {
                            var platform = _mapper.Map<Platform>(publishedDto);
                            repo.CreatePlatform(platform);
                            repo.SaveChanges();
                            Console.WriteLine("--> Platform added!");
                            return;
                        }
                        Console.WriteLine("--> Platform already exists");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"--> Não foi possível cadastrar plataforma {ex.Message}");
                    }
                }
            }
        }

        private static EventType DetermineEvent(string message)
        {
            var eventDto = JsonSerializer.Deserialize<GenericEventDto>(message);

            switch (eventDto.Event)
            {
                case "Published_Platform":
                    return EventType.PlatformPublished;
                default:
                    return EventType.Undetermined;
            }
        }
    }

    enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}
