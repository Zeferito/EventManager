using EventManager.Desktop.Api.Dto;
using Godot;
using System;

namespace EventManager.Desktop.Scenes.ConfirmacionEvento.Components.Scripts;

public partial class EventoDescripcionComponent : VBoxContainer
{
    [Export]
    private Label _labelNombreEvento;

    [Export]
    private Label _labelDescripcionEvento;

    private EventDto _eventDto;

    public EventDto EventDto
    {
        get => _eventDto;
        set => SetEventDto(value);
    }

    private void SetEventDto(EventDto value)
    {
        _eventDto = value;
        _labelNombreEvento.Text = value.Name;
        _labelDescripcionEvento.Text = value.Description;
    }
}
