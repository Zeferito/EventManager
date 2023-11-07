using EventManager.Desktop.Api.Entities;
using Godot;

namespace EventManager.Desktop.Scenes.DeleteEvento.Components.Scripts;

public partial class AgregableEventoItemComponent : VBoxContainer
{
    private Event _event;

    public Event Event
    {
        get => _event;
        set => SetEvent(value);
    }

    private Label _labelIdEvento;

    private Label _labelNombreEvento;

    private Label _labelDescripcionEvento;

    private Label _labelFechaInicio;

    private Label _labelFechaTermino;

    public override void _Ready()
    {
        _labelIdEvento = GetNode<Label>("HBoxContainer/LabelIdEvento");
        _labelNombreEvento = GetNode<Label>("HBoxContainer/LabelNombreEvento");
        _labelDescripcionEvento = GetNode<Label>("HBoxContainer/LabelDescripcionEvento");
        _labelFechaInicio = GetNode<Label>("HBoxContainer/LabelFechaInico");
        _labelFechaTermino = GetNode<Label>("HBoxContainer/LabelIdFechaTermino");
    }

    private void SetEvent(Event value)
    {
        _event = value;
        _labelIdEvento.Text = _event.Id.ToString();
        _labelNombreEvento.Text = _event.Name;
        _labelDescripcionEvento.Text = _event.Description;
        _labelFechaInicio.Text = _event.StartDate;
        _labelFechaTermino.Text = _event.EndDate;
    }
}