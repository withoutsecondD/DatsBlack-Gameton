using Newtonsoft.Json.Linq;

namespace Gameton;

public class LoggingHttpHandler : DelegatingHandler
{
    private ILogger _logger;
    public LoggingHttpHandler(ILogger logger) : base(new HttpClientHandler())
    {
        _logger = logger;
    }

    string TryPrettifyJson(string? json)
    {
        string prettifiedJson = "null";
        if (json != null)
            try
            {
                if (json.Length > 2000)
                    prettifiedJson = "{ LONG JSON }";
                else prettifiedJson = JToken.Parse(json).ToString();
            }
            catch (Exception ex)
            {
                _logger.LogDebug(nameof(LoggingHttpHandler), "Json parse error");
            }

        return prettifiedJson;
    }
    
    void LogHttpRequest(HttpRequestMessage req)
    {
        string message;
        if(req.Method == HttpMethod.Post)
        {
            string? reqContent = req.Content?.ReadAsStringAsync().GetAwaiter().GetResult();
            message = $"{req.Method} to {req.RequestUri}: {TryPrettifyJson(reqContent)}";
        }
        else message = $"{req.Method} to {req.RequestUri}";
        _logger.LogDebug($"REQUEST", message);
    }

    void LogHttpResponse(HttpResponseMessage res)
    {
        string resContent = res.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        string message = $"{res.StatusCode} ({(int)res.StatusCode}): {TryPrettifyJson(resContent)}";
        _logger.LogDebug($"RESPONSE", message);
    }

    protected override HttpResponseMessage Send(HttpRequestMessage req, CancellationToken cancellationToken)
    {
        LogHttpRequest(req);
        var res = base.Send(req, cancellationToken);
        LogHttpResponse(res);
        return res;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage req, CancellationToken cancellationToken)
    {
        LogHttpRequest(req);
        var res = await base.SendAsync(req, cancellationToken);
        LogHttpResponse(res);
        return res;
    }
}