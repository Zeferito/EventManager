using Godot;
using System;

public partial class ButtonSalon : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Pressed += () =>
		{
			//GetTree().ChangeSceneToFile("res://Scenas/AddEvento/add_evento_scene.tscn");
		};
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
