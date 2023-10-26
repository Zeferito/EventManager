using Godot;
using System;

public partial class DatabaseConnection : Node
{
	[Export]
	public string ConnectionString { get; set; }
}
