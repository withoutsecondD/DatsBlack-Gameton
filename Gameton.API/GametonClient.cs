global using System;
global using System.Collections.Generic;
global using System.Net.Http;
global using System.Threading;
global using System.Threading.Tasks;
global using System.Text;
global using DTLib.Logging;
global using DTLib.Filesystem;
global using DTLib.Extensions;
using System.Net.Http.Json;
using Gameton.DataModels;
using Gameton.DataModels.LongScan;
using Gameton.DataModels.Map;
using Gameton.DataModels.Scan;
using Gameton.DataModels.ShipCommand;

namespace Gameton;

public class GametonClient
{
    private const string base_url = "https://datsblack.datsteam.dev/api/";
    private HttpClient _http;
    private ILogger _logger;

    public GametonClient(string token, ILogger logger)
    {
        _logger = logger;
        _http = new HttpClient(new LoggingHttpHandler(_logger));
        _http.BaseAddress = new Uri(base_url);
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
    
    
    public async Task<TResponse> PostAsync<TResponse>(string requestUrl)
    {
        var response = await _http.PostAsync(requestUrl, new StringContent(""));
        TResponse? responseData = await response.Content.ReadFromJsonAsync<TResponse>();
        if (responseData != null)
            return responseData;
        
        throw new NullReferenceException($"POST {requestUrl} responded with null");
    }
    
    public async Task<TResponse> GetAsync<TResponse>(string requestUrl)
    {
        var response = await _http.GetAsync(requestUrl);
        TResponse? responseData = await response.Content.ReadFromJsonAsync<TResponse>();
        if (responseData != null)
            return responseData;
        
        throw new NullReferenceException($"GET {requestUrl} responded with null");
    }

    private void LogResponseErrors(ResponseBase response)
    {
        if (response.errors != null)
            foreach (var e in response.errors)
                _logger.LogWarn(nameof(TryRequestLongScanAsync), e);
    }
    
    /// <summary>
    /// Requests long scan with radius 60 on specified point. Has duration 15 ticks.
    /// Ships found by long scan will be visible by regular scan.
    /// </summary>
    /// <param name="x">coordinate of long scan center</param>
    /// <param name="y">coordinate of long scan center</param>
    /// <returns>LongScanResponse if long scan succeed, null if it is on cooldown</returns>
    public async Task<LongScanResponse?> TryRequestLongScanAsync(int x, int y)
    {
        var request = new LongScanRequest { x = x, y = y };
        var response = await PostAsync<LongScanResponse, LongScanRequest>("longScan", request);
        LogResponseErrors(response);
        return response.success ? response : null;
    }

    /// <summary>
    /// Sends commands to ships
    /// </summary>
    /// <param name="request">collection of ship commands</param>
    /// <returns>ShipCommandResponse if commands were valid, otherwise null</returns>
    public async Task<ShipCommandResponse?> TryRequestShipCommand(ShipCommandRequest request)
    {
        var response = await PostAsync<ShipCommandResponse, ShipCommandRequest>("shipCommand", request);
        LogResponseErrors(response);
        return response.success ? response : null;
    }
    
    /// <summary>
    /// Requests current game situation: your ships, spotted enemy ships, battle royal zone info
    /// </summary>
    /// <returns>ScanResponse or null on error</returns>
    public async Task<ScanResponse?> TryRequestScanAsync()
    {
        var response = await GetAsync<ScanResponse>("scan");
        LogResponseErrors(response);
        return response.success ? response : null;
    }

    /// <summary>
    /// Requests map of isles
    /// </summary>
    /// <returns>MapData or null on error</returns>
    public async Task<MapData?> TryRequestMap()
    {
        var response = await GetAsync<MapResponse>("map");
        LogResponseErrors(response);
        return response.success ? await GetAsync<MapData>(response.mapUrl) : null;
    }

    public async Task<bool> TryRegisterOnDeathmatch()
    {
        var response = await PostAsync<ResponseBase>("deathMatch/registration");
        LogResponseErrors(response);
        return response.success;
    }
    
    public async Task<bool> TryExitDeathmatch()
    {
        var response = await PostAsync<ResponseBase>("deathMatch/exitBattle");
        LogResponseErrors(response);
        return response.success;
    }
    
    public async Task<bool> TryRegisterOnBattleRoyal()
    {
        var response = await PostAsync<ResponseBase>("royalBattle/registration");
        LogResponseErrors(response);
        return response.success;
    }
}