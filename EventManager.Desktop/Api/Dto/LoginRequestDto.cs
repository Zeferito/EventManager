using System.Text.Json.Serialization;

namespace EventManager.Desktop.Api.Dto;

public class LoginRequestDto
{
    [JsonPropertyName("identifier")]
    public string Identifier { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }
}