using Godot;
using System;

public partial class ListaMaterialesContainer : VBoxContainer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Clear();
	}

	public void Clear()
	{
		foreach (Node node in GetChildren())
		{
			RemoveChild(node);
			node.QueueFree();
		}
	}
}
