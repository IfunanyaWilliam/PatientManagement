
namespace PatientManagement.Infrastructure.Services.Implementation
{
    using System.Collections.Concurrent;
    using Microsoft.Extensions.Logging;
    using Microsoft.AspNetCore.Http;
    using Application.Utilities;
    using System.Text.Json;
    using RestSharp;
    using System;
    
    
    using Application.Interfaces.Services;

    public class NetworkService : INetworkService
    {
        private readonly ILogger<NetworkService> _logger;


        public NetworkService(ILogger<NetworkService> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> GetAsync<TResponse>(string url, ConcurrentDictionary<string, string>? headers)
        {
            RestResponse respond = null;
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException("URL cannot be null or empty", nameof(url));
            }

            var client = new RestClient($"{url}");
            var request = new RestRequest();

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.AddHeader(header.Key, header.Value);
                }
            }

            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/json");
            var response = await client.GetAsync(request);

            if (!response.IsSuccessful || response.Content.Contains("error"))
            {
                _logger.LogError(
                    "HTTP GET request to {url} failed with status code {StatusCode}. Response: {Response}",
                    url,
                    response.StatusCode,
                    response.Content
                );

                throw new CustomException(
                    $"Request failed with status code {response.StatusCode}: {response.ErrorMessage}",
                    (int)response.StatusCode
                );
            }

            if (string.IsNullOrWhiteSpace(response.Content))
            {
                _logger.LogWarning("Empty response received from {Url}", url);
                return default(TResponse);
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            _logger.LogInformation($"NetworkService: Request to {url} responded with {response.Content}");
            var responseContent = JsonSerializer.Deserialize<TResponse>(respond.Content, options);
            return responseContent;
        }


        public async Task<TResponse> PostAsync<TResponse>(string url, object requestBody, ConcurrentDictionary<string, string>? headers)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(url))
                {
                    throw new ArgumentException("URL cannot be null or empty", nameof(url));
                }
                var client = new RestClient($"{url}");
                var request = new RestRequest();
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        request.AddHeader(header.Key, header.Value);
                    }
                }

                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddJsonBody(requestBody);
                var response = await client.PostAsync(request);

                if (!response.IsSuccessful)
                {
                    _logger.LogError(
                        "HTTP POST request to {url} failed with status code {StatusCode}. Response: {Response}",
                        url,
                        response.StatusCode,
                        response.Content
                    );
                    throw new CustomException(
                        $"Request failed with status code {response.StatusCode}: {response.ErrorMessage}",
                        (int)response.StatusCode
                    );
                }

                if (string.IsNullOrWhiteSpace(response.Content))
                {
                    _logger.LogWarning("Empty response received from {Url}", url);
                    return default(TResponse);
                }

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var responseContent = JsonSerializer.Deserialize<TResponse>(response.Content, options);
                return responseContent;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"NetworkService:{url} exception occurred {ex.StackTrace}");
                throw new CustomException($"Request Could not be processed at the moment", StatusCodes.Status500InternalServerError);
            }
        }
    }
}
