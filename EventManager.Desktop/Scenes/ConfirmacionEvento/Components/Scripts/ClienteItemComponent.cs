using EventManager.Database.Models.Entities;
using Godot;
using System;

public partial class ClienteItemComponent : HBoxContainer
{
	private Label _labelClienteNombre;

	private Cliente _cliente;

	public Cliente Cliente
	{
		get => _cliente;
		set => SetCliente(value);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_labelClienteNombre = GetNode<Label>("LabelClienteNombre");
	}

	private void SetCliente(Cliente value)
	{
		_cliente = value;
		_labelClienteNombre.Text = value.Nombre;
	}
}
