using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [Route("/api/c/[controller]")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICommandRepo _repo;

        public PlatformController(IMapper mapper, ICommandRepo repo)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetAllPlatforms() 
        {
           var platforms = _repo.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("--> Inbound POST # Command Service");

            return Ok("Inbound test of from Platforms Controler");
        }
    }
}
