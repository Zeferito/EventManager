using EventManager.Desktop.Api.Entities;
using Godot;
using System;

namespace EventManager.Desktop.Scenes.AdministrarCliente.RegistrarCliente.Components.Scripts;

public partial class ClienteItemContainer : HBoxContainer
{
  private Label _labelNombre;

    private Label _labelTelefono;
    private Client _client;

    public Client Client
    {
        get => _client;
        set => SetClient(value);
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _labelNombre = GetNode<Label>("LabelNombre");
        _labelTelefono = GetNode<Label>("LabelTelefono");
    }

    private void SetClient(Client value)
    {
        _client = value;
        _labelNombre.Text = _client.Name;
        _labelTelefono.Text = _client.Phone;
    }
}