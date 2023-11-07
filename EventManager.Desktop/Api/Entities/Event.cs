using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace EventManager.Desktop.Api.Entities;

public class Event
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("createdDate")]
    public string CreatedDate { get; set; }

    [JsonPropertyName("updatedDate")]
    public string UpdatedDate { get; set; }

    [JsonPropertyName("deletedDate")]
    public string DeletedDate { get; set; }

    [JsonPropertyName("version")]
    public int Version { get; set; }

    [JsonPropertyName("status")]
    public EventStatus Status { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("startDate")]
    public string StartDate { get; set; }

    [JsonPropertyName("endDate")]
    public string EndDate { get; set; }

    [JsonPropertyName("user")]
    public User User { get; set; }

    [JsonPropertyName("clients")]
    public List<Client> Clients { get; set; } = new();

    [JsonPropertyName("employees")]
    public List<Employee> Employees { get; set; } = new();

    [JsonPropertyName("rooms")]
    public List<Room> Rooms { get; set; } = new();

    [JsonPropertyName("eventToMaterial")]
    public List<EventToMaterial> EventToMaterial { get; set; } = new();

    public enum EventStatus
    {
        [EnumMember(Value = "active")]
        Active,

        [EnumMember(Value = "inactive")]
        Inactive
    }
}