using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable enable

namespace EventManager.Desktop.Api.Dto;

public class ClientDto
{
    [JsonPropertyName("version")]
    public int? Version { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("phone")]
    public string? Phone { get; set; }
}