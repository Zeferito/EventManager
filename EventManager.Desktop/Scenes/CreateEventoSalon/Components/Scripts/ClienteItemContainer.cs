using EventManager.Database.Models.Entities;
using Godot;
using System;

public partial class ClienteItemContainer : HBoxContainer
{
	private VBoxContainer _parentContainer;

	private Label _labelNombre;

	private Label _labelTelefono;

	private TextureButton _textureButtonEliminar;

	private Cliente _cliente;

	public Cliente Cliente
	{
		get => _cliente;
		set => SetCliente(value);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_parentContainer = GetParent<VBoxContainer>();
		_labelNombre = GetNode<Label>("LabelNombre");
		_labelTelefono = GetNode<Label>("LabelTelefono");
		_textureButtonEliminar = GetNode<TextureButton>("VBoxContainer/TextureButtonEliminar");

		_textureButtonEliminar.Pressed += () =>
		{
			_parentContainer.RemoveChild(this);
			QueueFree();
		};
	}

	private void SetCliente(Cliente value)
	{
		_cliente = value;
		_labelNombre.Text = _cliente.Nombre;
		_labelTelefono.Text = _cliente.Telefono;
	}
}
