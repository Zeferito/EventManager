using Godot;
using System;

namespace EventManager.Desktop.Scenes.Inicio.Components.Scripts;

public partial class AdministrarCliente : Button
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Pressed += () =>
        {
            GetTree().ChangeSceneToFile("res://Scenes/AdministrarCliente/administrar_cliente.tscn");
        };
    }
}
