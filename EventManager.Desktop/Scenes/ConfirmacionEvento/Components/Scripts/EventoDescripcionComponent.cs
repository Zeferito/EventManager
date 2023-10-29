using EventManager.Database.Models.Entities;
using Godot;
using System;

public partial class EventoDescripcionComponent : VBoxContainer
{
	[Export]
	private Label _labelNombreEvento;

	[Export]
	private Label _labelDescripcionEvento;

	private Evento _evento;

	public Evento Evento
	{
		get => _evento;
		set => SetEvento(value);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	private void SetEvento(Evento value)
	{
		_evento = value;
		_labelNombreEvento.Text = value.Nombre;
		_labelDescripcionEvento.Text = value.Descripcion;
	}
}
