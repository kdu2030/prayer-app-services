using RestSharp;

namespace PrayerAppServices.Files {
    public interface IFileServicesClient {
        Task<RestResponse<TResponseBody>> ExecuteAsync<TResponseBody>(RestRequest request);
    }
}
