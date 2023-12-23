using DTLib.Logging;
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
        string? reqContent = req.Content?.ReadAsStringAsync().GetAwaiter().GetResult();
        string prettifiedJson = reqContent == null ? "null" : JToken.Parse(reqContent).ToString();
        string message = $"{req.Method} to {req.RequestUri}: {prettifiedJson}";
        _logger.LogDebug($"REQUEST", message);
    }

    void LogHttpResponse(HttpResponseMessage res)
    {
        string? resContent = res.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        string prettifiedJson = JToken.Parse(resContent).ToString();
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