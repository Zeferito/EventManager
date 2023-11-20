using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable enable

namespace EventManager.Desktop.Api.Dto;

public class EmployeeDto
{
    [JsonPropertyName("version")]
    public int? Version { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}