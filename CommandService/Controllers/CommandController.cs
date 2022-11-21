using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [Route("api/c/platform/{platformId}/[controller]")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        private readonly ICommandRepository _repository;
        private readonly IMapper _mapper;

        public CommandController(ICommandRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommandReadDto>>> GetCommandsForPlatform(int platformId)
        {
            if (!await PlatformExists(platformId))
            {
                return NotFound();
            }

            var commands = await _repository.GetCommandsForPlatform(platformId);

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }

        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public async Task<ActionResult<CommandReadDto>> GetCommandForPlatform(int platformId, int commandId)
        {
            if (!await PlatformExists(platformId))
            {
                return NotFound();
            }

            var command = await _repository.GetCommand(platformId, commandId);

            if (command == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CommandReadDto>(command));
        }

        [HttpPost]
        public async Task<ActionResult<CommandReadDto>> CreateCommandForPlatform(int platformId, CommandCreateDto commandCreateDto)
        {
            if (!await PlatformExists(platformId))
            {
                return NotFound();
            }

            var command = _mapper.Map<Command>(commandCreateDto);

            await _repository.CreateCommand(platformId, command);
            await _repository.SaveChanges();

            var response = _mapper.Map<CommandReadDto>(command);

            return CreatedAtRoute
                (
                    nameof(GetCommandForPlatform),
                    new { platformId, commandId = response.Id },
                    response
                );
        }

        private async Task<bool> PlatformExists(int platformId) => await _repository.PlatformExists(platformId);
    }
}
