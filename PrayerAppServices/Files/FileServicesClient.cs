using RestSharp;

namespace PrayerAppServices.Files {
    public class FileServicesClient : IFileServicesClient {
        private readonly IRestClient _client;
        public string FileServicesUrl { get; private set; }

        public FileServicesClient(IConfiguration configuration) {
            FileServicesUrl = configuration["FileServices:Url"]
             ?? throw new NullReferenceException("File Upload URL cannot be null.");

            _client = new RestClient(FileServicesUrl);
        }

        public async Task<RestResponse<TResponseBody>> ExecuteAsync<TResponseBody>(RestRequest request) {
            return await _client.ExecuteAsync<TResponseBody>(request);
        }

    }
}
