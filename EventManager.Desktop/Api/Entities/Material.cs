using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace EventManager.Desktop.Api.Entities;

public class Material
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; }

    [JsonPropertyName("updatedDate")]
    public DateTime UpdatedDate { get; set; }

    [JsonPropertyName("version")]
    public int Version { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }

    [JsonPropertyName("type")]
    public MaterialType Type { get; set; }

    [JsonPropertyName("eventToMaterial")]
    public List<EventToMaterial> EventToMaterial { get; set; } = new();

    public enum MaterialType
    {
        [EnumMember(Value = "material")]
        Material,

        [EnumMember(Value = "equipment")]
        Equipment,

        [EnumMember(Value = "furniture")]
        Furniture
    }
}