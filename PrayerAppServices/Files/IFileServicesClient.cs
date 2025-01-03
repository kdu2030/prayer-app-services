using RestSharp;

namespace PrayerAppServices.Files {
    public interface IFileServicesClient {
        public string FileServicesUrl { get; }
        Task<RestResponse<TResponseBody>> ExecuteAsync<TResponseBody>(RestRequest request);
    }
}
