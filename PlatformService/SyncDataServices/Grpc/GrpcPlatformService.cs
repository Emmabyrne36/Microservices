using AutoMapper;
using Grpc.Core;
using PlatformService.Data;
using PlatformService.Models;

namespace PlatformService.SyncDataServices.Grpc
{
    public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
    {
        private readonly IPlatformRepository _repo;
        private readonly IMapper _mapper;

        public GrpcPlatformService(IPlatformRepository repo, IMapper mapper)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public override async Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
        {
            var response = new PlatformResponse();
            var platforms = await _repo.GetAllPlatforms();

            var res = _mapper.Map<PlatformResponse>(platforms);
            var res2 = _mapper.Map<IEnumerable<Platform>, PlatformResponse>(platforms);

            foreach (var plat in platforms)
            {
                response.Platform.Add(_mapper.Map<GrpcPlatformModel>(plat));
            }

            return response;
        }
    }
}
