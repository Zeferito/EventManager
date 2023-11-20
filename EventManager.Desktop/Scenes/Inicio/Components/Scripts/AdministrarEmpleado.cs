using Godot;
using System;

namespace EventManager.Desktop.Scenes.Inicio.Components.Scripts;
public partial class AdministrarEmpleado : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		 Pressed += () =>
        {
            GetTree().ChangeSceneToFile("res://Scenes/AdministrarEmpleado/administrar_empleado_scene.tscn");
        };
	}
}
