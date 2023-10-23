using Godot;
using System;

public partial class HandleButtons : Node
{
    [Export]
    private Button _buttonSalon;

    [Export]
    private Button _buttonAuditorio;

    [Export]
    private Button _buttonBack;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _buttonSalon.Pressed += () =>
        {
            GetTree()
                .ChangeSceneToFile("res://Scenes/CreateEventoSalon/create_evento_salon_scene.tscn");
        };

        _buttonAuditorio.Pressed += () =>
        {
            //GetTree().ChangeSceneToFile("res://Scenes/CreateEventoAuditorio/create_evento_auditorio_scene.tscn");
        };

        _buttonBack.Pressed += () =>
        {
            GetTree().ChangeSceneToFile("res://Scenes/Inicio/scena_pantalla_inicio.tscn");
        };
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }
}
