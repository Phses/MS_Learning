using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [Route("/api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICommandRepo _repo;

        public CommandController(IMapper mapper, ICommandRepo repo)
        {
            _mapper = mapper;
            _repo = repo;
        }
        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
        {
            if(!_repo.PlatformExist(platformId))
            {
                return NotFound();
            }

            var commands = _repo.GetCommandsForPlatform(platformId);
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }
        [HttpGet("{commandId}", Name = "GetCommandsForPlatform")]
        public ActionResult<CommandReadDto> GetCommand(int platformId, int commandId)
        {
            if(!_repo.PlatformExist(platformId))
            {
                return NotFound();
            }

            var command = _repo.GetCommand(platformId, commandId);

            if (command == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CommandReadDto>(command));
        }
        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(int platformId, CommandCreateDto commandDto) 
        {
            if(!_repo.PlatformExist(platformId))
            {
                return NotFound();
            }

            var command = _mapper.Map<Command>(commandDto);

            _repo.CreateCommand(platformId, command);
            _repo.SaveChanges();

            var commandRead = _mapper.Map<CommandReadDto>(command);

            return CreatedAtRoute("GetCommandsForPlatform",
                new { platformId, commandId = commandRead.Id }, commandRead);
        }
        
    }
}
