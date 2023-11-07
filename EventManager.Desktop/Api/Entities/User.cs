using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace EventManager.Desktop.Api.Entities;

public class User
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; }

    [JsonPropertyName("updatedDate")]
    public DateTime UpdatedDate { get; set; }

    [JsonPropertyName("version")]
    public int Version { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }

    [JsonPropertyName("roles")]
    public List<Role> Roles { get; set; } = new();

    [JsonPropertyName("events")]
    public List<Event> Events { get; private set; } = new();

    public enum Role
    {
        [EnumMember(Value = "admin")]
        Admin,

        [EnumMember(Value = "boss")]
        Boss,

        [EnumMember(Value = "staff")]
        Staff,

        [EnumMember(Value = "other")]
        Other
    }
}