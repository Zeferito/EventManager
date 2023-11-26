using Godot;
using System;

public partial class ButtonEliminar : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Pressed += () =>
		{
			GetTree().ChangeSceneToFile("res://Scenes/AdministrarCliente/EliminarCliente/eliminar_cliente.tscn");
		};
	}
}
