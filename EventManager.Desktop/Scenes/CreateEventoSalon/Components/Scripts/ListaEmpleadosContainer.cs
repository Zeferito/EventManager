using Godot;
using System;

public partial class ListaEmpleadosContainer : VBoxContainer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach(Node node in GetChildren()){
			RemoveChild(node);
			node.QueueFree();
		}
	}
}
