using Godot;
using System;

namespace EventManager.Desktop.Scenes.AdministrarEmpleado.OpcionesEmpleado.Components.Scripts;

public partial class ButtonRegistrar : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Pressed += () =>
		{
			GetTree().ChangeSceneToFile("res://Scenes/AdministrarEmpleado/RegistrarEmpleado/registrar_empleado.tscn");
		};
	}
}
