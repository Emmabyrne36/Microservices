using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        private readonly IPlatformRepository _repo;
        private readonly IMapper _mapper;

        public PlatformController(IPlatformRepository repo, IMapper mapper)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(repo));
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetAllPlatforms()
        {
            var platforms = _repo.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var platform = _repo.GetPlatformById(id);

            if (platform == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PlatformReadDto>(platform));
        }

        [HttpPost]
        public ActionResult<PlatformReadDto> CreatePlatform([FromBody] PlatformCreateDto platformCreateDto)
        {
            if (platformCreateDto == null)
            {
                return BadRequest();
            }

            var platform = _mapper.Map<Platform>(platformCreateDto);
            _repo.CreatePlatform(platform);
            _repo.SaveChanges();

            var result = _mapper.Map<PlatformReadDto>(platform);

            return CreatedAtRoute(nameof(GetPlatformById), new { Id = result.Id }, result);
        }
    }
}
