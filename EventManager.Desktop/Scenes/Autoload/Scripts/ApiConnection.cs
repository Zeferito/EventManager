using Godot;

namespace EventManager.Desktop.Scenes.Autoload.Scripts;

public partial class ApiConnection : Node
{
	public AuthDetails AuthDetails { get; set; }

	[Export]
	public string Url { get; set; }

	public override void _Ready()
	{
		AuthDetails = GetNode<AuthDetails>("AuthDetails");
	}
}