using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformController2 : ControllerBase
    {
        private readonly IPlatformService _platformService;

        public PlatformController2(IPlatformService platformService)
        {
            _platformService = platformService ?? throw new ArgumentNullException(nameof(platformService));
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetAllPlatforms()
        {
            var platforms = _platformService.GetAllPlatforms();

            return Ok(platforms);
        }

        [HttpGet("{id}", Name = "GetPlatformById2")]
        public ActionResult<PlatformReadDto> GetPlatformById2(int id)
        {
            try
            {
                return Ok(_platformService.GetPlatformById(id));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public ActionResult<PlatformReadDto> CreatePlatform([FromBody] PlatformCreateDto platformCreateDto)
        {
            try
            {
                var result = _platformService.CreatePlatform(platformCreateDto);
                return CreatedAtRoute(nameof(GetPlatformById2), new { result.Id }, result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }


    
    
    
    
    
    
    
    
    
    
    // ####################################################################################################################################





    public interface IPlatformService
    {
        IEnumerable<PlatformReadDto> GetAllPlatforms();
        PlatformReadDto GetPlatformById(int id);
        PlatformReadDto CreatePlatform(PlatformCreateDto platformCreateDto);
    }

    public class PlatformService : IPlatformService
    {
        private readonly IPlatformRepository _repo;
        private readonly IMapper _mapper;

        public PlatformService(IPlatformRepository repo, IMapper mapper)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(repo));
        }

        public IEnumerable<PlatformReadDto> GetAllPlatforms()
        {
            var platforms = _repo.GetAllPlatforms();

            return _mapper.Map<IEnumerable<PlatformReadDto>>(platforms);
        }

        public PlatformReadDto GetPlatformById(int id)
        {
            var platform = _repo.GetPlatformById(id);

            if (platform == null)
            {
                throw new Exception("The platform was not found");
            }

            return _mapper.Map<PlatformReadDto>(platform);
        }

        public PlatformReadDto CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            if (platformCreateDto == null)
            {
                throw new Exception("The request value was null.");
            }

            var platform = _mapper.Map<Platform>(platformCreateDto);
            _repo.CreatePlatform(platform);
            _repo.SaveChanges();

            var result = _mapper.Map<PlatformReadDto>(platform);

            return result;
        }
    }
}
