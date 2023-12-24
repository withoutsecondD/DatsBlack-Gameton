using Newtonsoft.Json.Linq;

namespace Gameton;

public class LoggingHttpHandler : DelegatingHandler
{
    private ILogger _logger;
    public LoggingHttpHandler(ILogger logger) : base(new HttpClientHandler())
    {
        _logger = logger;
    }
    
    void LogHttpRequest(HttpRequestMessage req)
    {
        string message;
        if(req.Method == HttpMethod.Post)
        {
            string? reqContent = req.Content?.ReadAsStringAsync().GetAwaiter().GetResult();
            string prettifiedJson = "null";
            if (reqContent != null)
                try
                {
                    if (reqContent.Length > 2000)
                        prettifiedJson = "{ LONG JSON }";
                    else prettifiedJson = JToken.Parse(reqContent).ToString();
                }
                catch (Exception ex)
                {
                    _logger.LogError(nameof(LoggingHttpHandler), ex);
                }

            message = $"{req.Method} to {req.RequestUri}: {prettifiedJson}";
        }
        else message = $"{req.Method} to {req.RequestUri}";
        _logger.LogDebug($"REQUEST", message);
    }

    void LogHttpResponse(HttpResponseMessage res)
    {
        string resContent = res.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        string prettifiedJson = "null";
        try
        {
            if (resContent.Length > 2000)
                prettifiedJson = "{ LONG JSON }";
            else prettifiedJson = JToken.Parse(resContent).ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(nameof(LoggingHttpHandler), ex);
        }
        string message = $"{res.StatusCode} ({(int)res.StatusCode}): {prettifiedJson}";
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