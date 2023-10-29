using Godot;
using System;

public partial class ApplicationProperties : Node
{
	private int _maxFramerate = 60;

	public int MaxFramerate
	{
		get => _maxFramerate;
		set => SetMaxFramerate(value);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Engine.MaxFps = MaxFramerate;
	}

	private void SetMaxFramerate(int value)
	{
		_maxFramerate = value;
		Engine.MaxFps = value;
	}
}
