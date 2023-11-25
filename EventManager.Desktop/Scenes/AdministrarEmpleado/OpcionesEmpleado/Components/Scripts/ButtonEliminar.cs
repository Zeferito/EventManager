using Godot;
using System;

namespace EventManager.Desktop.Scenes.AdministrarEmpleado.OpcionesEmpleado.Components.Scripts;

public partial class ButtonEliminar : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Pressed += () =>
		{
			GetTree().ChangeSceneToFile("res://Scenes/Inicio/scena_pantalla_inicio.tscn");
		};
	}
}
