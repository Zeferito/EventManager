using EventManager.Desktop.Api.Entities;
using Godot;
using System;

namespace EventManager.Desktop.Scenes.ConfirmacionEvento.Components.Scripts;

public partial class ClienteItemComponent : HBoxContainer
{
    private Label _labelClienteNombre;

    private Client _client;

    public Client Client
    {
        get => _client;
        set => SetCliente(value);
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _labelClienteNombre = GetNode<Label>("LabelClienteNombre");
    }

    private void SetCliente(Client value)
    {
        _client = value;
        _labelClienteNombre.Text = value.Name;
    }
}
