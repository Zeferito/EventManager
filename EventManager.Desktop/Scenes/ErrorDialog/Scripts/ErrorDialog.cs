using Godot;
using System;
namespace EventManager.Desktop.Scenes.ErrorDialog.Scripts;
public partial class ErrorDialog : Control
{

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Hide();
	}

	 public void SetErrorMessage(string message)
    {
        Label _error = GetNode<Label>("ErrorMessageLabel");
        _error.Text = message;
    }
}
