using AutoMapper;
using CommandService.Models;
using Grpc.Net.Client;
using PlatformService;

namespace CommandService.SyncDataServices.Grpc
{
    public class PlatformDataClient : IPlatformDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public PlatformDataClient(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public IEnumerable<Platform> ReturnAllPlatforms()
        {
            var service = _configuration["GrpcPlatform"];
            Console.WriteLine($"--> Calling GRPC Service {service}");

            var channel = GrpcChannel.ForAddress(service);
            var client = new GrpcPlatform.GrpcPlatformClient(channel);
            var request = new GetAllRequest();

            IEnumerable<Platform> result = new List<Platform>();
            try
            {
                var reply = client.GetAllPlatforms(request);
                result = _mapper.Map<IEnumerable<Platform>>(reply.Platform);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not call GRPC Server {ex.Message}");
            }

            return result;
        }
    }
}
