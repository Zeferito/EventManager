using EventManager.Desktop.Api.Dto;
using Godot;
using System;

public partial class Global : Node
{
    public EventDto EventToSend { get; set; }

    public int EventIdToUpdate { get; set; }
}
