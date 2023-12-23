using System.Text.Json.Serialization;
using DTLib.Logging;
using Newtonsoft.Json.Linq;

namespace Gameton;

public class GametonClient
{
    private const string base_url = "https://datsblack.datsteam.dev/";
    private HttpClient _http;

    public GametonClient(string token)
    {
        _http = new HttpClient(new LoggingHttpHandler())
        {
            BaseAddress = new Uri(base_url)
        };
        _http.DefaultRequestHeaders.Add("X-API-Key", token);
    }
}

public class LoggingHttpHandler : DelegatingHandler
{
    public LoggingHttpHandler(HttpMessageHandler innerHandler) : base(innerHandler)
    { }

    public LoggingHttpHandler()
    { }

    public ILogger Logger = new CompositeLogger(
            new ConsoleLogger(),
            new FileLogger("logs", "gameton")
        );
    
    void LogHttpRequest(HttpRequestMessage req)
    {
        string? reqContent = req.Content?.ReadAsStringAsync().GetAwaiter().GetResult();
        string prettifiedJson = reqContent == null ? "null" : JToken.Parse(reqContent).ToString();
        string message = $"{req.Method} to {req.RequestUri}: {prettifiedJson}";
        Logger.LogDebug($"REQUEST", message);
    }

    void LogHttpResponse(HttpResponseMessage res)
    {
        string? resContent = res.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        string prettifiedJson = JToken.Parse(resContent).ToString();
        string message = $"{res.StatusCode} ({(int)res.StatusCode}): {prettifiedJson}";
        Logger.LogDebug($"RESPONSE", message);
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