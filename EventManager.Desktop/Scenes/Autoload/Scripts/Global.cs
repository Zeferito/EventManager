using EventManager.Database.Models.Entities;
using Godot;
using System;

public partial class Global : Node
{
	public Evento EventoToSend { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
}
