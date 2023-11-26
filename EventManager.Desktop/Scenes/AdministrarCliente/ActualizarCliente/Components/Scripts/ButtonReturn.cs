using Godot;
using System;

namespace EventManager.Desktop.Scenes.AdministrarCliente.ActualizarCliente.Components.Scripts;

public partial class ButtonReturn : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
				Pressed += () =>
		{
			GetTree().ChangeSceneToFile("res://Scenes/AdministrarCliente/OpcionesCliente/scena_opciones_cliente.tscn");
		};
	}
}