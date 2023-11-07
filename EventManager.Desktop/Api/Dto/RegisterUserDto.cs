using System.Text.Json.Serialization;

namespace EventManager.Desktop.Api.Dto;

public class RegisterUserDto
{
    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }
}