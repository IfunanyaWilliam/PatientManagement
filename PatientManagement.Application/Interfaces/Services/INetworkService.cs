

namespace PatientManagement.Application.Interfaces.Services
{
    using System.Collections.Concurrent;

    public interface INetworkService
    {
        Task<TResponse> GetAsync<TResponse>(string url, ConcurrentDictionary<string, string>? headers);
        Task<TResponse> PostAsync<TResponse>(string url, object requestBody, ConcurrentDictionary<string, string>? headers);
    }
}
