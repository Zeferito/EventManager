using Godot;
using System;

public partial class ButtonEditar : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Pressed += () =>
		{
			GetTree()
				.ChangeSceneToFile("res://Scenes/EditEvent/scena_editar_evento.tscn");
		};
	}
}
