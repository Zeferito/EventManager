using Godot;
using System;

namespace EventManager.Desktop.Scenes.CreateEventoAuditorio.Components.Scripts
{
    public partial class HandleBottomButtons : Node
    {
        [Export]
        private Button _buttonConfirm;

        [Export]
        private Button _buttonCancel;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _buttonConfirm.Pressed += () =>
            {
                GetTree()
                    .ChangeSceneToFile(
                        "res://Scenes/ConfirmacionEvento/confirmacion_evento_scene.tscn"
                    );
            };
            _buttonCancel.Pressed += () =>
            {
                GetTree().ChangeSceneToFile("res://Scenes/Inicio/scena_pantalla_inicio.tscn");
            };
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta) { }
    }
}
