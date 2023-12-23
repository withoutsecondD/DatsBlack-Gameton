

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

    public LoggingHttpHandler() : base()
    { }
    
    void LogHttpRequest(HttpRequestMessage req)
    {
        // Console.WriteLine($"REQUEST {req.Method} to {req.RequestUri}: " +
        //                   req.Content?.ReadAsStringAsync().GetAwaiter().GetResult());
    }

    void LogHttpResponse(HttpResponseMessage res)
    {
        // Console.WriteLine($"RESPONSE {res.StatusCode} ({(int)res.StatusCode}) from {res.RequestMessage?.RequestUri}: " +
        //                   res.Content.ReadAsStringAsync().GetAwaiter().GetResult());
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