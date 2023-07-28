using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataService.http;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;
        private readonly IPlatformDataClient _platformDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public PlatformController(IPlatformRepo repository, IMapper mapper, IPlatformDataClient platformDataClient, IMessageBusClient messageBusClient)
        {
            _repository = repository;
            _mapper = mapper;
            _platformDataClient = platformDataClient;
            _messageBusClient = messageBusClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetAll() 
        {
            var platforms = _repository.GetAllPlatform();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }

        [HttpGet("{id}", Name = "GetById")]
        public ActionResult<PlatformReadDto> GetById(int id) 
        {
            var platform = _repository.GetPlatformById(id);
            if(platform != null)
            {
                return Ok(platform);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> Create(PlatformCreateDto platformCreate)
        {
            var platform = _mapper.Map<Platform>(platformCreate);

            _repository.CreatePlatform(platform);
            _repository.SaveChanges();

            //Send sync

            var platformRead = _mapper.Map<PlatformReadDto>(platform);

            try
            {
                Console.WriteLine("---> Send data sync");
               await _platformDataClient.SendPlatformToCommandService(platformRead);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"---> Its not possible sync data {ex.Message}");
            }

            //Send Async
            var platformPublishDto = _mapper.Map<PlatformPublishedDto>(platformRead);
            platformPublishDto.Event = "Published_Platform";
            try
            {
                Console.WriteLine("---> Send data Async");
                _messageBusClient.PublishNewPlatform(platformPublishDto);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"---> Its not possible async data {ex.Message}");
            }

            return CreatedAtRoute("GetById", new { id = platformRead.Id }, platformRead);

        }
    }
}
