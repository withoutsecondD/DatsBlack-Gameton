using System.Text.Json.Serialization;

namespace Gameton.DataModels.Map; 

public class Island
{
    [JsonRequired] public List<List<int>> map { get; set; } // Каждый вложенный список показывает "строку" острова
    [JsonRequired] public List<int> start { get; set; } // [x, y] Координаты начала (верхнего левого угла) острова
}