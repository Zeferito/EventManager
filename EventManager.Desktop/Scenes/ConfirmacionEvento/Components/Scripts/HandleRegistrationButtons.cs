using Godot;
using System;

public partial class HandleRegistrationButtons : Node
{
	[Export]
	private Button _buttonRegister;
	[Export]
	private Button _buttonCancel;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_buttonRegister.Pressed += () =>
		{
			GetTree().ChangeSceneToFile("res://Scenes/Inicio/scena_pantalla_inicio.tscn");
		};

		_buttonCancel.Pressed += () =>
		{
			GetTree().ChangeSceneToFile("res://Scenes/AddEvento/add_evento_scene.tscn");
		};
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) { }
}
