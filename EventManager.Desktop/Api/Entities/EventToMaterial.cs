using System.Text.Json.Serialization;

namespace EventManager.Desktop.Api.Entities;

public class EventToMaterial
{
    [JsonPropertyName("amountReserved")]
    public int AmountReserved { get; set; }

    [JsonPropertyName("event")]
    public Event Event { get; set; }

    [JsonPropertyName("material")]
    public Material Material { get; set; }
}