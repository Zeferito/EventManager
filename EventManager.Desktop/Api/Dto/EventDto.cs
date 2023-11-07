using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable enable

namespace EventManager.Desktop.Api.Dto;

public class EventDto
{
    [JsonPropertyName("version")]
    public int? Version { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("startDate")]
    public string StartDate { get; set; }

    [JsonPropertyName("endDate")]
    public string EndDate { get; set; }

    [JsonPropertyName("userId")]
    public int UserId { get; set; }

    [JsonPropertyName("clientIds")]
    public List<int> ClientIds { get; set; } = new();

    [JsonPropertyName("employeeIds")]
    public List<int> EmployeeIds { get; set; } = new();

    [JsonPropertyName("roomIds")]
    public List<int> RoomIds { get; set; } = new();

    [JsonPropertyName("materialReserved")]
    public List<MaterialReserved> MaterialsReserved { get; set; } = new List<MaterialReserved>();

    public class MaterialReserved
    {
        [JsonPropertyName("materialId")]
        public int MaterialId { get; set; }

        [JsonPropertyName("amount")]
        public int Amount { get; set; }
    }
}