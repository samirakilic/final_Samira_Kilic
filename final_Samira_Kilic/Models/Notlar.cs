using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace final_Samira_Kilic;

public class OpenWeatherResponse
{
    [JsonPropertyName("weather")]
    public List<WeatherInfo> Weather { get; set; } = new();

    [JsonPropertyName("main")]
    public MainInfo Main { get; set; } = new();
}

public class WeatherInfo
{
    [JsonPropertyName("description")]
    public string Description { get; set; } = "";

    [JsonPropertyName("icon")]
    public string Icon { get; set; } = "";
}

public class MainInfo
{
    [JsonPropertyName("temp")]
    public double Temp { get; set; }
}
