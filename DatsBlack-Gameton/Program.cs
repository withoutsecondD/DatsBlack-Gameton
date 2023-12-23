global using System;
global using System.Collections.Generic;
global using System.Net.Http;
global using System.Threading;
global using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using DTLib.Filesystem;
using Gameton.DataModels.Map;

namespace Gameton;

public static class Program
{
    public static void Main()
    {
        // string? token = Environment.GetEnvironmentVariable("GAMETON_TOKEN");
        // if(string.IsNullOrEmpty(token))
        //     throw new Exception("can't get value of environment variable GAMETON_TOKEN");
        // var client = new GametonClient(token);

        string jsonString = File.ReadAllText("map.json");
        MapResponse map = JsonSerializer.Deserialize<MapResponse>(jsonString)!;

        MapRenderer.Render(map);
    }
}
