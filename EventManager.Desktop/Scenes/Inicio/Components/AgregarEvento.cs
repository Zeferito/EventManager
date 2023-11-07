using Godot;
using System;

public partial class AgregarEvento : Button
{
	public override void _Ready()
	{
		Pressed += () =>
		{
			GetTree().ChangeSceneToFile("res://Scenes/AdministrarEvento/add_evento_scene.tscn");
		};
	}
}
