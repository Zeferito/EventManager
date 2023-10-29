using Godot;
using System;

public partial class ButtonAuditorio : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Pressed += () =>
		{
			//GetTree().ChangeSceneToFile("res://Scenes/CreateEventoAuditorio/create_evento_auditorio_scene.tscn");
		};
	}
}
