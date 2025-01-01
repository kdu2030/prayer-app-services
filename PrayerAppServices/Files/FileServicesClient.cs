using RestSharp;

namespace PrayerAppServices.Files {
    public class FileServicesClient : IFileServicesClient {
        private readonly IRestClient _client;

        public FileServicesClient(IConfiguration configuration) {
            string fileServicesUrl = configuration["FileServices:Url"]
             ?? throw new NullReferenceException("File Upload URL cannot be null.");

            _client = new RestClient(fileServicesUrl);
        }

        public async Task<RestResponse<TResponseBody>> ExecuteAsync<TResponseBody>(RestRequest request) {
            return await _client.ExecuteAsync<TResponseBody>(request);
        }

    }
}
