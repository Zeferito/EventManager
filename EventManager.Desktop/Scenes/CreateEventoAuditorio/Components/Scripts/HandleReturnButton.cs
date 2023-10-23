using Godot;
using System;

namespace EventManager.Desktop.Scenes.CreateEventoAuditorio.Components.Scripts
{
    public partial class HandleReturnButton : Node
    {
        [Export]
        private Button _buttonBack;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _buttonBack.Pressed += () =>
            {
                //GetTree().ChangeSceneToFile("res://Scenes/Inicio/scena_pantalla_inicio.tscn");
                GetTree().ChangeSceneToFile("res://Scenes/AddEvento/add_evento_scene.tscn");
            };
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta) { }
    }
}
