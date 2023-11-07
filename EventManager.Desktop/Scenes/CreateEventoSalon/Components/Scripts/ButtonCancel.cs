using Godot;
using System;

public partial class ButtonCancel : Button
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Pressed += () =>
        {
            GetTree().ChangeSceneToFile("res://Scenes/AdministrarEvento/add_evento_scene.tscn");
        };
    }
}
