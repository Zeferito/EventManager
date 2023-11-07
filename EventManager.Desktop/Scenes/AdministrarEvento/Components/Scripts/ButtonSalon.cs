using Godot;
using System;

public partial class ButtonSalon : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Pressed += () =>
		{
			GetTree()
				.ChangeSceneToFile("res://Scenes/CreateEventoSalon/create_evento_salon_scene.tscn");
		};
	}
}
