using System.Net.Http.Json;
using System.Text.Json.Serialization;
using DTLib.Logging;
using Gameton.DataModels.LongScan;

namespace Gameton;

public class GametonClient
{
    private const string base_url = "https://datsblack.datsteam.dev/api/";
    private HttpClient _http;
    private ILogger _logger;

    public GametonClient(string token, ILogger logger)
    {
        _logger = logger;
        _http = new HttpClient(new LoggingHttpHandler(_logger))
        {
            BaseAddress = new Uri(base_url)
        };
        _http.DefaultRequestHeaders.Add("X-API-Key", token);
    }

    public async Task<TResponse> PostAsync<TResponse, TRequest>(string requestUrl, TRequest requestData)
    {
        var reqJsonContent = JsonContent.Create(requestData);
        var response = await _http.PostAsync(requestUrl, reqJsonContent);
        TResponse? responseData = await response.Content.ReadFromJsonAsync<TResponse>();
        if (responseData != null)
            return responseData;
        throw new NullReferenceException($"POST {requestUrl} responded with null");
    }
    
    public async Task<TResponse> GetAsync<TResponse, TRequest>(string requestUrl)
    {
        var response = await _http.GetAsync(requestUrl);
        TResponse? responseData = await response.Content.ReadFromJsonAsync<TResponse>();
        if (responseData != null)
            return responseData;
        throw new NullReferenceException($"GET {requestUrl} responded with null");
    }
    
    /// <summary>
    /// Requests long scan with radius 60 on specified point. Has duration 15 ticks.
    /// </summary>
    /// <param name="x">coordinate of long scan center</param>
    /// <param name="y">coordinate of long scan center</param>
    /// <returns>LongScanResponse if long scan succeed, null if it is on cooldown</returns>
    public async Task<LongScanResponse?> TryRequestLongScanAsync(int x, int y)
    {
        var request = new LongScanRequest { x = x, y = y };
        var response = await PostAsync<LongScanResponse, LongScanRequest>("longScan", request);
        if (response.success) 
            return response;

        if (response.errors != null)
            foreach (var e in response.errors)
                _logger.LogWarn(nameof(TryRequestLongScanAsync), e);
        return null;
    }
}