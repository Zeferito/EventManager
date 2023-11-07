using Godot;

namespace EventManager.Desktop.Scenes.Autoload.Scripts;

public partial class AuthDetails : Node
{
	[Export]
	public string AuthToken { get; set; }
}