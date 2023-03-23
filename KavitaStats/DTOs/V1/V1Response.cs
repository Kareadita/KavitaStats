using System.Text.Json.Serialization;

namespace KavitaStats.DTOs.V1;

public class V1Response
{
    public bool Success { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Error { get; set; }
}